alter PROCEDURE Dispensador1LadoB --Un sp para cada lado de cada dispensador
as
begin

SELECT M.IdManguera,CONCAT(D.idDispensador,L.Letra, RANK() over(order by M.IdManguera)) AS Manguera,C.Nombre As Combustible, N.numeracion AS NumeracionAyer, C.PrecioGalon
FROM tblManguera AS M
INNER JOIN tblCombustible AS C ON M.idCombustible = C.idCombustible
INNER JOIN tblLado AS L ON M.idLado = L.idLado
INNER JOIN tblDispensador AS D ON L.idDispensador = D.idDispensador
INNER JOIN tblNumeracion AS N ON M.idManguera = N.idManguera
WHERE D.idDispensador = 1 and L.Letra = 'B' and N.idNumeracion = (SELECT TOP(1) X.idNumeracion FROM tblNumeracion AS X where X.idManguera = M.idManguera order by X.idNumeracion desc)
end

ALTER PROCEDURE Numeraciones
(@IdDispensador int, @Lado char(1))
AS
BEGIN

if EXISTS (SELECT * FROM tblDispensadorEntrega WHERE CONVERT(varchar(15), Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103) and IdDispensador = @IdDispensador)
BEGIN

SELECT CONCAT(DE.IdDispensador,L.Letra,RANK() over(order by M.IdManguera)) AS Manguera,C.Nombre AS Combustible,
isnull((SELECT Z.numeracion from tblNumeracion AS Z WHERE Z.idNumeracion = (SELECT TOP(1) X.idNumeracion from tblNumeracion AS X WHERE X.idNumeracion != (SELECT TOP(1) Y.idNumeracion from tblNumeracion AS Y WHERE Y.idManguera = M.idManguera order by Y.idNumeracion desc) and  X.idManguera = M.idManguera order by X.idNumeracion desc)), N.numeracion) AS NumeracionAyer,
N.numeracion AS NumeracionHoy,
isnull((N.Numeracion- (SELECT Z.numeracion from tblNumeracion AS Z WHERE Z.idNumeracion = (SELECT TOP(1) X.idNumeracion from tblNumeracion AS X WHERE X.idNumeracion != (SELECT TOP(1) Y.idNumeracion from tblNumeracion AS Y WHERE Y.idManguera = M.idManguera order by Y.idNumeracion desc) and  X.idManguera = M.idManguera order by X.idNumeracion desc))),0) AS GalonesHoy
, N.PrecioGalon
,isnull(ROUND(( N.PrecioGalon * (N.Numeracion- (SELECT Z.numeracion from tblNumeracion AS Z WHERE Z.idNumeracion = (SELECT TOP(1) X.idNumeracion from tblNumeracion AS X WHERE X.idNumeracion != (SELECT TOP(1) Y.idNumeracion from tblNumeracion AS Y WHERE Y.idManguera = M.idManguera order by Y.idNumeracion desc) and  X.idManguera = M.idManguera order by X.idNumeracion desc)))),2),0) AS GeneradoHoy
, M.idManguera
FROM tblNumeracion AS N
INNER JOIN tblManguera AS M ON N.idManguera = M.idManguera
INNER JOIN tblDispensadorEntrega AS DE ON N.IdDispensadorEntrega = DE.IdEntrega
INNER JOIN tblCombustible AS C ON M.idCombustible = C.idCombustible
INNER JOIN tblLado AS L ON M.idLado = L.idLado
INNER JOIN (
         SELECT MAX(AN.idNumeracion) AS idNumeracion, AN.idManguera
           FROM tblNumeracion AS AN
		  INNER JOIN tblDispensadorEntrega AS DE ON AN.IdDispensadorEntrega = DE.IdEntrega
		  WHERE De.IdDispensador = @IdDispensador
		  GROUP BY AN.idManguera
       ) AS AN ON N.idNumeracion = AN.idNumeracion
WHERE L.Letra = @Lado
END

ELSE
BEGIN

SELECT CONCAT(D.idDispensador,L.Letra, RANK() over(order by M.IdManguera)) AS Manguera,C.Nombre As Combustible, N.numeracion AS NumeracionAyer, NULL As NumeracionHoy, 0 AS GalonesHoy, C.PrecioGalon, 0 AS GeneradoHoy, M.IdManguera
FROM tblManguera AS M
INNER JOIN tblCombustible AS C ON M.idCombustible = C.idCombustible
INNER JOIN tblLado AS L ON M.idLado = L.idLado
INNER JOIN tblDispensador AS D ON L.idDispensador = D.idDispensador
INNER JOIN tblNumeracion AS N ON M.idManguera = N.idManguera
WHERE D.idDispensador = @IdDispensador and L.Letra = @Lado and N.idNumeracion = (SELECT TOP(1) X.idNumeracion FROM tblNumeracion AS X where X.idManguera = M.idManguera order by X.idNumeracion desc)

END

END

ALTER PROCEDURE UltimaEfectivoEntregado
(@IdDispensador int)
AS
BEGIN

SELECT TOP(1) isnull(EfectivoEntregado + Vales + Credito,0) AS Valor
FROM tblDispensadorEntrega
WHERE IdDispensador = @IdDispensador
ORDER BY IdEntrega desc
END
go

ALTER PROCEDURE UltimoEfectivoGenerado
(@IdDispensador int)
AS
BEGIN

SELECT TOP(1) isnull(TotalGenerado,0) AS Valor
FROM tblDispensadorEntrega
WHERE IdDispensador = @IdDispensador
ORDER BY IdEntrega desc
END

ALTER PROCEDURE UltimosIngresosEntregados
(@IdDispensador int)
AS
BEGIN

SELECT TOP(1) TotalGenerado, EfectivoEntregado, Vales, Credito, IdEmpleado
FROM tblDispensadorEntrega
WHERE IdDispensador = @IdDispensador
ORDER BY IdEntrega desc
END

ALTER PROCEDURE VentaCombustibleEntreFechas
(@FechaInicio datetime, @FechaFin datetime)
AS
BEGIN

SELECT isnull(SUM(TotalGenerado),0) AS Valor
FROM tblDispensadorEntrega
WHERE Fecha BETWEEN @FechaInicio AND @FechaFin

END

CREATE PROCEDURE HayNumeracionHoy
(@IdDispensador int)
AS
BEGIN

if EXISTS (SELECT * FROM tblDispensadorEntrega WHERE CONVERT(varchar(15), Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103) and IdDispensador = @IdDispensador)
BEGIN
SELECT 1 as Valor
END

ELSE
BEGIN
SELECT 0 AS Valor
END

END


ALTER PROCEDURE Bomberos
AS
BEGIN
SELECT [idEmpleado], [idTipoEmpleado], [Cedula], Nombre + ' ' + Apellidos As Nombre, [Apellidos], [Arqueo], [Salario], [Activo], [Telefono], [Direccion], [FechaRegistro]
FROM tblEmpleado
WHERE idTipoEmpleado = 2
END

Alter PROCEDURE EmpleadosConTipo
AS
BEGIN

SELECT E.idEmpleado, TE.tipoEmpleado , E.Cedula AS Cédula, E.Nombre, E.Apellidos, E.Arqueo, E.Salario, (case when E.Activo = 1 then 'Activado' else 'Desactivado' end) AS Estado, E.Telefono, E.Direccion, convert(varchar(15), E.FechaRegistro, 103) AS [Registro]
FROM tblEmpleado AS E
INNER JOIN tblTipoEmpleado AS TE ON E.idTipoEmpleado = TE.idTipoEmpleado
ORDER BY Activo DESC

end
go

ALTER PROCEDURE EmpleadosConTipoPorNombre
(@NombreCompleto varchar(100))
AS
BEGIN

SELECT E.idEmpleado, TE.tipoEmpleado , E.Cedula AS Cédula, E.Nombre, E.Apellidos, E.Arqueo, E.Salario, (case when E.Activo = 1 then 'Activado' else 'Desactivado' end) AS Estado, E.Telefono, E.Direccion, convert(varchar(15), E.FechaRegistro, 103) AS [Registro]
FROM tblEmpleado AS E
INNER JOIN tblTipoEmpleado AS TE ON E.idTipoEmpleado = TE.idTipoEmpleado
WHERE CONCAT(E.Nombre,' ',E.Apellidos) LIKE '%' + @NombreCompleto +'%'
ORDER BY Activo DESC

end

CREATE PROCEDURE TiposDeEmpleados
AS
BEGIN

SELECT idTipoEmpleado, tipoEmpleado
FROM tblTipoEmpleado

END

CREATE PROCEDURE InsertarEmpleado
(@IdTipoEmpleado int, @Cedula char(11), @Nombre varchar(25), @Apellidos varchar(50), @Salario decimal(10,2), @Estado bit, @Telefono char(10), @Direccion varchar(50))
AS
BEGIN

INSERT INTO tblEmpleado([idTipoEmpleado], [Cedula], [Nombre], [Apellidos], [Arqueo], [Salario], [Activo], [Telefono], [Direccion], [FechaRegistro])
VALUES (@IdTipoEmpleado, @Cedula, @Nombre, @Apellidos, 0, @Salario, @Estado, @Telefono, @Direccion, GETDATE())

END

CREATE PROCEDURE ActualizarEmpleado
(@IdEmpleado int, @IdTipoEmpleado int, @Nombre varchar(25), @Apellidos varchar(50), @Salario decimal(10,2), @Telefono char(10), @Direccion varchar(50))
AS
BEGIN

UPDATE tblEmpleado set idTipoEmpleado = @IdTipoEmpleado, Nombre = @Nombre, Apellidos = @Apellidos, Salario = @Salario, Telefono = @Telefono, Direccion = @Direccion
WHERE idEmpleado = @IdEmpleado;

END

CREATE PROCEDURE EmpleadoPorID
(@IdEmpleado int)
AS
BEGIN

SELECT [idEmpleado], [idTipoEmpleado], [Cedula], [Nombre], [Apellidos], [Arqueo], [Salario], [Activo], [Telefono], [Direccion], [FechaRegistro]
FROM tblEmpleado
WHERE idEmpleado = @IdEmpleado

END

CREATE PROCEDURE CambiarEstadoEmpleado
(@IdEmpleado int)
AS
BEGIN

UPDATE tblEmpleado set Activo = (CASE WHEN Activo = 1 THEN 0 ELSE 1 END)
WHERE idEmpleado = @IdEmpleado
END

CREATE PROCEDURE InsertarTipoEmpleado
(@TipoEmpleado varchar(50))
AS
BEGIN

INSERT INTO tblTipoEmpleado(tipoEmpleado) VALUES(@TipoEmpleado)

END

ALTER PROCEDURE Combustibles
AS
BEGIN

SELECT C.[idCombustible], [Nombre], [Cantidad], [PrecioGalon], PC.Fecha As UltimaCompra
FROM tblCombustible AS C
INNER JOIN tblProveedorCombustible AS PC ON C.idCombustible = PC.idCombustible
INNER JOIN (SELECT MAX(APC.idProvComb) AS idProveedorCombustible, AC.idCombustible
           FROM tblCombustible AS AC
		   INNER JOIN tblProveedorCombustible AS APC ON APC.idCombustible=  AC.idCombustible
		  GROUP BY AC.idCombustible) AS Aux ON PC.idProvComb = Aux.idProveedorCombustible
WHERE C.idCombustible != 1

END

CREATE PROCEDURE CambiarPrecioCombustible
(@IdCombustible int, @PrecioPorGalon decimal(10,2))
AS
BEGIN

UPDATE tblCombustible 
SET PrecioGalon = @PrecioPorGalon 
WHERE idCombustible = @IdCombustible

END

CREATE PROCEDURE InsertarProveedor
(@RNC char(9), @Nombre varchar(50), @Telefono char(10))
AS
BEGIN

INSERT INTO tblProveedor([RNC], [Nombre], [Telefono], [FechaRegistro])
VALUES (@RNC, @Nombre, @Telefono, GETDATE())

END

CREATE PROCEDURE Proveedores
AS
BEGIN

SELECT [idProveedor], [RNC], [Nombre], [Telefono], [FechaRegistro]
FROM tblProveedor

END

ALTER PROCEDURE InsertarCompraCombustible
(@IdProveedor int, @IdCombustible int, @PrecioPorGalon decimal(10,2), @CantidadGalones decimal(10,2))
AS
BEGIN
BEGIN TRY
BEGIN TRANSACTION

INSERT INTO tblProveedorCombustible([idProveedor], [idCombustible], [PrecioPorGalon], [CantidadGalones], [Fecha])
VALUES (@IdProveedor, @IdCombustible, @PrecioPorGalon, @CantidadGalones, GETDATE())

declare @IdProveedorCombustible int = (SELECT IDENT_CURRENT('tblProveedorCombustible'))
declare @NombreCombustible varchar(50) = (SELECT Nombre FROM tblCombustible WHERE idCombustible = @IdCombustible)
declare @Concepto nvarchar(100) = (select CONCAT('Compra de ',@CantidadGalones,' galones de ',@NombreCombustible,' por RD$',@PrecioPorGalon))
declare @Monto decimal(10,2) = (select @PrecioPorGalon * @CantidadGalones)
declare @Fecha datetime = (SELECT GETDATE())

EXECUTE InsertarEgreso 1, @IdProveedorCombustible, @Concepto, @Monto, @Fecha
EXECUTE ActualizarCantidadCombustible @IdCombustible, @CantidadGalones

COMMIT
END TRY
BEGIN CATCH

ROLLBACK

END CATCH
END

ALTER PROCEDURE InsertarEgreso
(@IdTipoEgreso int, @idOrigen int, @Concepto nvarchar(100), @Monto decimal(10,2), @Fecha datetime)
AS
BEGIN

INSERT INTO tblEgreso([IdTipoEgreso], [idOrigen], [Concepto], [Monto], [Fecha])
VALUES (@IdTipoEgreso, @idOrigen, @Concepto, @Monto, @Fecha)

END

CREATE PROCEDURE ActualizarCantidadCombustible
(@IdCombustible int, @Cantidad decimal(10,2))
AS
BEGIN

UPDATE tblCombustible 
set Cantidad = Cantidad + @Cantidad 
WHERE idCombustible = @IdCombustible

END
go

ALTER PROCEDURE EgresosConTipo
AS
BEGIN

SELECT E.[Concepto], TE.TipoEgreso,E.[Monto], E.[Fecha]
FROM tblEgreso AS E
INNER JOIN tblTipoEgreso AS TE ON E.IdTipoEgreso = TE.IdTipoEgreso
ORDER BY E.Fecha DESC

END

CREATE PROCEDURE EgresosPorConcepto
(@Concepto varchar(100))
AS
BEGIN

SELECT E.[Concepto], TE.TipoEgreso,E.[Monto], E.[Fecha]
FROM tblEgreso AS E
INNER JOIN tblTipoEgreso AS TE ON E.IdTipoEgreso = TE.IdTipoEgreso
WHERE E.Concepto LIKE '%' + @Concepto + '%'
ORDER BY E.Fecha DESC

END

CREATE PROCEDURE IngresosConTipo
AS
BEGIN
SELECT 
I.[Concepto], TI.TipoIngreso,I.[Monto], I.[Fecha]
FROM tblIngreso AS I
INNER JOIN tblTipoIngreso AS TI ON I.IdTipoIngreso= TI.IdTipoIngreso
ORDER BY I.Fecha DESC
END

CREATE PROCEDURE IngresosPorConcepto
(@Concepto varchar(100))
AS
BEGIN
SELECT 
I.[Concepto], TI.TipoIngreso,I.[Monto], I.[Fecha]
FROM tblIngreso AS I
INNER JOIN tblTipoIngreso AS TI ON I.IdTipoIngreso= TI.IdTipoIngreso
WHERE I.Concepto LIKE '%' + @Concepto + '%'
ORDER BY I.Fecha DESC
END


CREATE PROCEDURE TotalTipoIngreso15Dias
AS
BEGIN

SELECT TI.TipoIngreso, SUM(I.Monto) AS [Monto Total]
FROM tblIngreso AS I
INNER JOIN tblTipoIngreso AS TI ON I.IdTipoIngreso = TI.IdTipoIngreso
WHERE I.Fecha >= DATEADD(day,-15,GETDATE())
GROUP BY TI.TipoIngreso

END

ALTER PROCEDURE TotalTipoEgreso15Dias
AS
BEGIN

SELECT TE.TipoEgreso, SUM(E.Monto) AS [Monto Total]
FROM tblEgreso AS E
INNER JOIN tblTipoEgreso AS TE ON E.IdTipoEgreso = TE.IdTipoEgreso
WHERE E.Fecha >= DATEADD(day,-15,GETDATE())
GROUP BY Te.TipoEgreso

END

ALTER PROCEDURE TotalEgresos15Dias
AS
BEGIN

SELECT isnull(SUM(E.Monto),0) AS Valor
FROM tblEgreso AS E
WHERE E.Fecha >= DATEADD(day,-15,GETDATE())

END

ALTER PROCEDURE TotalIngresos15Dias
AS
BEGIN

SELECT isnull(SUM(I.Monto),0) AS Valor
FROM tblIngreso AS I
WHERE I.Fecha >= DATEADD(day,-15,GETDATE())

END

CREATE PROCEDURE TIposEgresosInsertar
AS
BEGIN

SELECT [IdTipoEgreso], [TipoEgreso]
FROM tblTipoEgreso
WHERE IdTipoEgreso not in(1,2,3)

END
GO

ALTER PROCEDURE TiposIngresosInsertar
AS
BEGIN

SELECT [IdTipoIngreso], [TipoIngreso]
FROM tblTipoIngreso
WHERE IdTipoIngreso not in(1,2)

END


CREATE PROCEDURE InsertarTipoDeIngreso
(@TipoIngreso varchar(50))
AS
BEGIN

INSERT INTO tblTipoIngreso(TipoIngreso)
VALUES (@TipoIngreso)

END

ALTER PROCEDURE InsertarNumeracion
(@IdManguera int, @Numeracion decimal(10,2), @PrecioGalon decimal(10,2), @IdDispensadorEntrega int)
AS
BEGIN
BEGIN TRY
BEGIN TRANSACTION
declare @NumeracionDeAyer decimal(10,2) = (SELECT Numeracion FROM tblNumeracion AS X WHERE idNumeracion = (SELECT TOP(1) Z.idNumeracion FROM tblNumeracion AS Z WHERE Z.idManguera = @IdManguera ORDER BY idNumeracion DESC))

INSERT INTO tblNumeracion([idManguera], [numeracion], [PrecioGalon], [IdDispensadorEntrega])
VALUES (@IdManguera, @Numeracion, @PrecioGalon, @IdDispensadorEntrega)

declare @IdCombustible int = (SELECT X.IdCombustible FROM tblManguera AS X WHERE X.idManguera = @IdManguera)
declare @GalonesDeHoy decimal(10,2) = (SELECT @NumeracionDeAyer - @Numeracion)

EXECUTE ActualizarCantidadCombustible @IdCombustible, @GalonesDeHoy
UPDATE tblDispensadorEntrega set TotalGenerado = TotalGenerado + (-1*@GalonesDeHoy * @PrecioGalon) WHERE IdEntrega = @IdDispensadorEntrega
COMMIT

END TRY

BEGIN CATCH

ROLLBACK

END CATCH
END

ALTER PROCEDURE InsertarDispensadorEntrega
(@IdDispensador int, @IdEmpleado int, @EfectivoEntregado decimal(10,2), @Vales decimal(10,2), @Credito decimal(10,2))
AS
BEGIN
BEGIN TRY
BEGIN TRANSACTION

INSERT INTO [dbo].[tblDispensadorEntrega]([IdDispensador], [IdEmpleado], [EfectivoEntregado], [Vales], [Credito], [Fecha])
VALUES(@IdDispensador, @IdEmpleado, @EfectivoEntregado, @Vales, @Credito,GETDATE())

declare @IdEntrega int = (SELECT IDENT_CURRENT('tblDispensadorEntrega'))
declare @Concepto varchar(100) = (SELECT CONCAT('Monto generado por el dispensador #',@IdDispensador,' entregado por ',(SELECT CONCAT(Nombre,' ',Apellidos) FROM tblEmpleado WHERE IdEmpleado = @IdEmpleado)))
declare @montoTotal decimal(10,2) = @EfectivoEntregado + @Vales + @Credito
declare @fecha datetime = (SELECT GETDATE())

EXECUTE InsertarIngreso 1, @IdEntrega, @Concepto, @montoTotal, @fecha

COMMIT
END TRY

BEGIN CATCH

ROLLBACK

END CATCH
END

ALTER PROCEDURE InsertarIngreso
(@IdTipoIngreso int, @IdOrigen int, @Concepto varchar(100), @Monto decimal(10,2), @Fecha datetime)
AS
BEGIN

INSERT INTO tblIngreso([IdTipoIngreso], [IdOrigen], [Concepto], [Monto], [Fecha])
VALUES (@IdTipoIngreso, @IdOrigen, @Concepto, @Monto, @Fecha)

END

ALTER PROCEDURE ActualizarArqueo
(@IdEntrega int)
AS
BEGIN

DECLARE @MontoEntregado decimal(10,2) = (SELECT Z.EfectivoEntregado + Z.Vales + Z.Credito FROM tblDispensadorEntrega AS Z WHERE Z.IdEntrega = @IdEntrega)
DECLARE @MontoGenerado decimal(10,2) = (SELECT Z.TotalGenerado FROM tblDispensadorEntrega AS Z WHERE Z.IdEntrega = @IdEntrega)
DECLARE @Arqueo decimal(10,2) = @MontoEntregado - @MontoGenerado
DECLARE @IdEmpleado int = (SELECT Z.IdEmpleado FROM tblDispensadorEntrega AS Z WHERE Z.IdEntrega = @IdEntrega)

UPDATE tblEmpleado SET Arqueo = Arqueo + @Arqueo WHERE idEmpleado = @IdEmpleado

END



ALTER PROCEDURE UltimaEntrega
AS
BEGIN

SELECT isnull(MAX(IdEntrega),0) AS Valor FROM tblDispensadorEntrega

END


ALTER PROCEDURE EmpleadosActivos
AS
BEGIN

SELECT isnull(COUNT(case when Activo = 1 then 1 end),0) AS Valor
FROM tblEmpleado

END

ALTER PROCEDURE PagoDeNomina
AS
BEGIN

SELECT isnull(SUM(case when Activo = 1 then salario + arqueo end),0) AS Valor
FROM tblEmpleado


END

ALTER PROCEDURE SobranteAcumulado
AS
BEGIN

SELECT ISNULL(SUM(case when Arqueo > 0 then arqueo else 0 end),0) AS Valor
FROM tblEmpleado

END

ALTER PROCEDURE FaltanteAcumulado
AS
BEGIN

SELECT ISNULL(SUM(case when Arqueo < 0 then arqueo else 0 end),0) AS Valor
FROM tblEmpleado

END

CREATE PROCEDURE HayVentaHoy
AS
BEGIN

IF EXISTS(SELECT * FROM tblDispensadorEntrega WHERE CONVERT(varchar(15), Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103))
BEGIN

SELECT 0 AS Valor

END

ELSE
BEGIN

SELECT 1 AS Valor

END

END
GO

CREATE PROCEDURE GananciaPorCombustible
(@Dia int)
AS
BEGIN

SELECT C.Nombre AS Combustible, 
isnull(ROUND(( N.PrecioGalon * (N.Numeracion- (SELECT Z.numeracion from tblNumeracion AS Z WHERE Z.idNumeracion = (SELECT TOP(1) X.idNumeracion from tblNumeracion AS X WHERE X.idNumeracion != (SELECT TOP(1) Y.idNumeracion from tblNumeracion AS Y WHERE Y.idManguera = M.idManguera order by Y.idNumeracion desc) and  X.idManguera = M.idManguera order by X.idNumeracion desc)))),2),0) AS GeneradoHoy
into #Venta FROM tblNumeracion AS N
INNER JOIN tblManguera AS M ON N.idManguera = M.idManguera
INNER JOIN tblDispensadorEntrega AS DE ON N.IdDispensadorEntrega = DE.IdEntrega
INNER JOIN tblCombustible AS C ON M.idCombustible = C.idCombustible
INNER JOIN (
         SELECT MAX(AN.idNumeracion) AS idNumeracion, AN.idManguera
           FROM tblNumeracion AS AN
		  INNER JOIN tblDispensadorEntrega AS DE ON AN.IdDispensadorEntrega = DE.IdEntrega
		  GROUP BY AN.idManguera
       ) AS AN ON N.idNumeracion = AN.idNumeracion
	   WHERE CONVERT(varchar(15), DE.Fecha, 103) = CONVERT(varchar(15), GETDATE() - @Dia, 103)

													  
select Combustible,SUM(GeneradoHoy) AS Generado from #Venta
group by Combustible

drop table #Venta

END

CREATE PROCEDURE GalonesDeCombustible
(@Dia int, @IdDispensador int)
AS
BEGIN

SELECT C.Nombre AS Combustible, 
isnull(((N.Numeracion- (SELECT Z.numeracion from tblNumeracion AS Z WHERE Z.idNumeracion = (SELECT TOP(1) X.idNumeracion from tblNumeracion AS X WHERE X.idNumeracion != (SELECT TOP(1) Y.idNumeracion from tblNumeracion AS Y WHERE Y.idManguera = M.idManguera order by Y.idNumeracion desc) and  X.idManguera = M.idManguera order by X.idNumeracion desc)))),0) AS GeneradoHoy
into #Venta FROM tblNumeracion AS N
INNER JOIN tblManguera AS M ON N.idManguera = M.idManguera
INNER JOIN tblDispensadorEntrega AS DE ON N.IdDispensadorEntrega = DE.IdEntrega
INNER JOIN tblCombustible AS C ON M.idCombustible = C.idCombustible
INNER JOIN (
         SELECT MAX(AN.idNumeracion) AS idNumeracion, AN.idManguera
           FROM tblNumeracion AS AN
		  INNER JOIN tblDispensadorEntrega AS DE ON AN.IdDispensadorEntrega = DE.IdEntrega
		  GROUP BY AN.idManguera
       ) AS AN ON N.idNumeracion = AN.idNumeracion
	   WHERE CONVERT(varchar(15), DE.Fecha, 103) = CONVERT(varchar(15), GETDATE() - @Dia, 103) and DE.IdDispensador = @IdDispensador

	   select Combustible, SUM(GeneradoHoy) AS Galones from #Venta GROUP BY Combustible

	   drop table #Venta

END

CREATE PROCEDURE EntradaHoy
AS
BEGIN

SELECT isnull(SUM(Monto),0) AS Valor FROM tblIngreso WHERE  CONVERT(varchar(15), Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103)

END

CREATE PROCEDURE SalidaHoy
AS
BEGIN

SELECT isnull(SUM(Monto),0) AS Valor FROM tblEgreso WHERE  CONVERT(varchar(15), Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103)

END

ALTER PROCEDURE NetoHoy
AS
BEGIN

declare @Entrada decimal(12,2) = (SELECT isnull(SUM(Monto),0) AS Valor FROM tblIngreso WHERE  CONVERT(varchar(15), Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103))
declare @Salida decimal(12,2) = (SELECT isnull(SUM(Monto),0) AS Valor FROM tblEgreso WHERE  CONVERT(varchar(15), Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103))
declare @Neto decimal(12,2) = @Entrada - @Salida

SELECT isnull(@Neto,0) AS Valor

END

CREATE PROCEDURE CombustibleRestante
AS
BEGIN

SELECT Nombre As Combustible, Cantidad As Galones FROM tblCombustible where idCombustible != 1

END

ALTER PROCEDURE MovimientosDelDia
AS
BEGIN

SELECT Concepto,Monto ,Movimiento, [Tipo de Movimiento], Fecha FROM (

select I.Concepto, I.Monto,'Ingreso' AS [Movimiento] ,TI.TipoIngreso AS [Tipo de Movimiento], I.Fecha
from tblIngreso AS I
inner join tblTipoIngreso AS TI ON I.IdTipoIngreso = Ti.IdTipoIngreso
WHERE CONVERT(varchar(15), I.Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103)

UNION

select E.Concepto, (E.Monto * -1), 'Egreso' AS [Movimiento] ,TE.TipoEgreso AS [Tipo de Movimiento], E.Fecha
from tblEgreso AS E
INNER JOIN tblTipoEgreso AS TE ON E.IdTipoEgreso = TE.IdTipoEgreso
WHERE CONVERT(varchar(15), E.Fecha, 103) = CONVERT(varchar(15), GETDATE(), 103)

) Movimientos

ORDER BY Movimiento asc, Monto desc

END

ALTER PROCEDURE TodosLosMovimientos
AS
BEGIN

SELECT ROW_NUMBER() OVER(ORDER BY Movimiento asc, Monto desc) AS Numero,Concepto,Monto ,Movimiento, TipoMovimiento, Fecha FROM (

select I.Concepto, I.Monto,'Ingreso' AS [Movimiento] ,TI.TipoIngreso AS [TipoMovimiento], I.Fecha
from tblIngreso AS I
inner join tblTipoIngreso AS TI ON I.IdTipoIngreso = Ti.IdTipoIngreso

UNION

select E.Concepto, (E.Monto * -1), 'Egreso' AS [Movimiento] ,TE.TipoEgreso AS [TipoMovimiento], E.Fecha
from tblEgreso AS E
INNER JOIN tblTipoEgreso AS TE ON E.IdTipoEgreso = TE.IdTipoEgreso

) Movimientos



END

CREATE PROCEDURE IngresosPorFecha
(@FechaInicio datetime, @FechaFinal datetime)
AS
BEGIN

select ROW_NUMBER() OVER(ORDER BY I.Fecha) AS Numero,TI.TipoIngreso,I.Concepto, I.Monto, I.Fecha
from tblIngreso AS I
inner join tblTipoIngreso AS TI ON I.IdTipoIngreso = Ti.IdTipoIngreso
WHERE I.Fecha BETWEEN @FechaInicio and @FechaFinal

END
go

CREATE PROCEDURE EgresosPorFecha
(@FechaInicio datetime, @FechaFinal datetime)
AS
BEGIN

select ROW_NUMBER() OVER(ORDER BY E.Fecha) AS Numero,TE.TipoEgreso,E.Concepto, E.Monto, E.Fecha
from tblEgreso AS E
inner join tblTipoEgreso AS TE ON E.IdTipoEgreso = TE.IdTipoEgreso
WHERE E.Fecha BETWEEN @FechaInicio and @FechaFinal

END

ALTER PROCEDURE MovimientosPorFecha
(@FechaInicio datetime, @FechaFinal datetime)
AS
BEGIN

SELECT ROW_NUMBER() oVER(ORDER BY Movimiento asc, Monto desc) AS Numero,Concepto,Monto ,Movimiento, [Tipo de Movimiento], Fecha FROM (

select I.Concepto, I.Monto,'Ingreso' AS [Movimiento] ,TI.TipoIngreso AS [Tipo de Movimiento], I.Fecha
from tblIngreso AS I
inner join tblTipoIngreso AS TI ON I.IdTipoIngreso = Ti.IdTipoIngreso
WHERE I.Fecha BETWEEN @FechaInicio and @FechaFinal

UNION

select E.Concepto, (E.Monto * -1), 'Egreso' AS [Movimiento] ,TE.TipoEgreso AS [Tipo de Movimiento], E.Fecha
from tblEgreso AS E
INNER JOIN tblTipoEgreso AS TE ON E.IdTipoEgreso = TE.IdTipoEgreso
WHERE E.Fecha BETWEEN @FechaInicio and @FechaFinal

) Movimientos


END

ALTER PROCEDURE SelectNomina
AS
BEGIN

SELECT ROW_NUMBER() OVER(ORDER BY FechaRegistro) AS Numero, CONCAT(Nombre,' ',Apellidos) AS Nombre, TE.tipoEmpleado ,Salario, Arqueo, (Salario + Arqueo) AS TotalGanado
FROM tblEmpleado AS E
INNER JOIN tblTipoEmpleado AS TE ON E.idTipoEmpleado = TE.idTipoEmpleado
WHERE Activo = 1

END

ALTER PROCEDURE PagarNomina
AS
BEGIN

BEGIN TRY

BEGIN TRANSACTION

SELECT idEmpleado,CONCAT(Nombre,' ',Apellidos) AS Nombre, Salario, Arqueo into #Nomina
FROM tblEmpleado
WHERE Activo = 1

declare @Contador int = (SELECT COUNT(*) from #Nomina)

WHILE(@Contador > 0)
	BEGIN

		declare @IdEmpleado int = (SELECT TOP(1) IdEmpleado from #Nomina)
		declare @NombreEmpleado varchar(100) = (SELECT TOP(1) Nombre from #Nomina)
		declare @Salario decimal(10,2) = (SELECT TOP(1) Salario from #Nomina)
		declare @ConceptoSalario varchar(100) = (SELECT CONCAT('Pago de salario al empleado ', @NombreEmpleado))
		declare @Fecha datetime = (SELECT GETDATE())
		EXEC InsertarEgreso 2, @IdEmpleado, @ConceptoSalario, @Salario, @Fecha
		declare @ConceptoArqueo varchar(100)

		declare @Arqueo decimal(10,2) = (SELECT TOP(1) Arqueo FROM #Nomina)

		IF (@Arqueo > 0)
			BEGIN

				set @ConceptoArqueo = (SELECT CONCAT('Pago de sobrante al empleado ', @NombreEmpleado))
				EXEC InsertarEgreso 3, @IdEmpleado, @ConceptoArqueo, @Arqueo, @Fecha

			END
		ELSE IF (@Arqueo < 0)
			BEGIN
				set @Arqueo = @Arqueo * -1
				set @ConceptoArqueo = (SELECT CONCAT('Pago de faltante del empleado ', @NombreEmpleado))
				EXEC InsertarIngreso 2, @IdEmpleado, @ConceptoArqueo, @Arqueo, @Fecha

			END

		delete from #Nomina WHERE idEmpleado = @IdEmpleado
		set @Contador = @Contador-1
	END

DROP TABLE #Nomina

UPDATE tblEmpleado SET Arqueo = 0 WHERE Activo = 1

COMMIT

END TRY

BEGIN CATCH

ROLLBACK

END CATCH

END

CREATE PROCEDURE DiasDeNomina
AS
BEGIN

SELECT TOP(1) DATEDIFF(DAY,Fecha,GETDATE()) AS Valor
FROM tblEgreso
WHERE IdTipoEgreso = 2
order by Fecha desc

END

ALTER PROCEDURE NumeracionesEntreFecha
(@FechaInicio datetime, @FechaFinal datetime)
AS
BEGIN

SELECT CONCAT(DE.IdDispensador,L.Letra,RANK() over(partition by D.NombreDispensador ,L.Letra order by M.IdManguera)) AS Manguera,C.Nombre AS Combustible,
isnull((SELECT TOP(1) Z.numeracion from tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE,X.Fecha) <= @FechaInicio and Z.idManguera = M.idManguera order by Z.idNumeracion desc),0) AS NumeracionAyer,
N.numeracion AS NumeracionHoy,
isnull((N.Numeracion - (SELECT TOP(1) Z.numeracion from tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE, X.Fecha) <= @FechaInicio and Z.idManguera = M.idManguera order by Z.idNumeracion desc)),0) AS GalonesHoy
, isnull(ROUND((SELECT AVG(Z.PrecioGalon) FROM tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE,X.Fecha) BETWEEN @FechaInicio and @FechaFinal and Z.idManguera = M.idManguera),2),0) AS PrecioGalon
,isnull(ROUND(( (SELECT AVG(Z.PrecioGalon) FROM tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE,X.Fecha) BETWEEN @FechaInicio and @FechaFinal and Z.idManguera = M.idManguera) * (N.Numeracion- (SELECT TOP(1) Z.numeracion from tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE, X.Fecha) <= @FechaInicio and Z.idManguera = M.idManguera order by Z.idNumeracion desc))),2),0) AS GeneradoHoy
, M.idManguera
FROM tblNumeracion AS N
INNER JOIN tblManguera AS M ON N.idManguera = M.idManguera
INNER JOIN tblDispensadorEntrega AS DE ON N.IdDispensadorEntrega = DE.IdEntrega
INNER JOIN tblDispensador AS D ON DE.IdDispensador = D.idDispensador
INNER JOIN tblCombustible AS C ON M.idCombustible = C.idCombustible
INNER JOIN tblLado AS L ON M.idLado = L.idLado
WHERE CONVERT(VARCHAR(15), DE.Fecha,103) = CONVERT(VARCHAR(15),@FechaFinal, 103)

END

ALTER PROCEDURE NumeracionPorFecha
(@Fecha datetime)
AS
BEGIN

SELECT CONCAT(DE.IdDispensador,L.Letra,RANK() over(partition by D.NombreDispensador ,L.Letra order by M.IdManguera)) AS Manguera,C.Nombre AS Combustible,
isnull((SELECT TOP(1) Z.numeracion from tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE,X.Fecha) < @Fecha and Z.idManguera = M.idManguera order by Z.idNumeracion desc),0) AS NumeracionAyer,
N.numeracion AS NumeracionHoy,
isnull((N.Numeracion - (SELECT TOP(1) Z.numeracion from tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE, X.Fecha) < @Fecha and Z.idManguera = M.idManguera order by Z.idNumeracion desc)),0) AS GalonesHoy
, N.PrecioGalon
,isnull(ROUND((N.PrecioGalon * (N.Numeracion- (SELECT TOP(1) Z.numeracion from tblNumeracion AS Z INNER JOIN tblDispensadorEntrega AS X ON Z.IdDispensadorEntrega = X.IdEntrega WHERE CONVERT(DATE, X.Fecha) < @Fecha and Z.idManguera = M.idManguera order by Z.idNumeracion desc))),2),0) AS GeneradoHoy
, M.idManguera
FROM tblNumeracion AS N
INNER JOIN tblManguera AS M ON N.idManguera = M.idManguera
INNER JOIN tblDispensadorEntrega AS DE ON N.IdDispensadorEntrega = DE.IdEntrega
INNER JOIN tblDispensador AS D ON DE.IdDispensador = D.idDispensador
INNER JOIN tblCombustible AS C ON M.idCombustible = C.idCombustible
INNER JOIN tblLado AS L ON M.idLado = L.idLado
WHERE CONVERT(VARCHAR(15), DE.Fecha,103) = CONVERT(VARCHAR(15),@Fecha, 103)

END

ALTER PROCEDURE DatosNumeracionPorFecha
(@Fecha datetime)
AS
BEGIN

SELECT CONCAT(E.Nombre,' ',E.Apellidos) AS Empleado, DE.TotalGenerado, DE.EfectivoEntregado, DE.Vales, DE.Credito,(DE.EfectivoEntregado + DE.Vales + DE.Credito - DE.TotalGenerado) AS Arqueo, DE.IdDispensador
FROM tblDispensadorEntrega AS DE
INNER JOIN tblEmpleado AS E ON DE.IdEmpleado = E.idEmpleado
WHERE CONVERT(DATE,DE.Fecha) = CONVERT(DATE,@Fecha)

END

ALTER PROCEDURE DatosNumeracionEntreFechas
(@FechaInicio datetime, @FechaFinal datetime)
AS
BEGIN

SELECT CONVERT(VARCHAR(3),COUNT(DISTINCT CONCAT(E.Nombre,' ',E.Apellidos))) AS Empleado, SUM(DE.TotalGenerado) AS TotalGenerado, SUM(DE.EfectivoEntregado) AS EfectivoEntregado, SUM(DE.Vales) AS Vales, SUM(DE.Credito) AS Credito, SUM(DE.EfectivoEntregado + DE.Vales + DE.Credito - DE.TotalGenerado) AS Arqueo, DE.IdDispensador
FROM tblDispensadorEntrega AS DE
INNER JOIN tblEmpleado AS E ON DE.IdEmpleado = E.idEmpleado
WHERE CONVERT(DATE,DE.Fecha) BETWEEN CONVERT(DATE, @FechaInicio) and CONVERT(DATE,@FechaFinal) and DE.IdDispensador IN (SELECT Z.IdDispensador FROM tblDispensadorEntrega AS Z WHERE CONVERT(DATE,Z.Fecha) = CONVERT(DATE,@FechaFinal))
GROUP BY DE.IdDispensador

END

CREATE PROCEDURE UltimasCompras
AS
BEGIN

SELECT P.Nombre AS Proveedor, C.Nombre AS Combustible, PC.CantidadGalones, PC.PrecioPorGalon ,PC.Fecha
FROM tblCombustible AS C
INNER JOIN tblProveedorCombustible AS PC ON C.idCombustible = PC.idCombustible
INNER JOIN (
SELECT MAX(idProvComb) AS idProvComb, idCombustible
FROM tblProveedorCombustible
GROUP BY idCombustible
) AS APC ON PC.idProvComb = APC.idProvComb
INNER JOIN tblProveedor AS P ON PC.idProveedor = P.idProveedor

END