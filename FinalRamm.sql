-- DDL
CREATE DATABASE FinalRamm;
USE [master]
GO
CREATE LOGIN [usrfinalramm] WITH PASSWORD=N'12345678',
	DEFAULT_DATABASE=[FinalRamm],
	CHECK_EXPIRATION=OFF,
	CHECK_POLICY=ON
GO
USE [FinalRamm]
GO
CREATE USER [usrfinalramm] FOR LOGIN [usrfinalramm]
GO
USE [FinalRamm]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrfinalramm]
GO

DROP TABLE VentaDetalle;
DROP TABLE Venta;
DROP TABLE CompraDetalle;
DROP TABLE Compra;
DROP TABLE Cliente;
DROP TABLE Empleado;
DROP TABLE Usuario;
DROP TABLE Proveedor;
DROP TABLE Producto;

CREATE TABLE Producto(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	codigo VARCHAR(10) NOT NULL,
	descripcion VARCHAR(250) NOT NULL,
	unidadMedida VARCHAR(20) NOT NULL,
	existencias DECIMAL NOT NULL DEFAULT 0,
	marca VARCHAR(20) NOT NULL,
	precioVenta DECIMAL NOT NULL CHECK (precioVenta > 0),
	usuarioRegistro VARCHAR(100) NULL DEFAULT SUSER_NAME(),
	registroActivo BIT NULL DEFAULT 1,
	fechaRegistro DATETIME NULL DEFAULT GETDATE()
);
CREATE TABLE Proveedor(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	nit BIGINT NOT NULL,
	razonSocial VARCHAR(100) NOT NULL,
	direccion VARCHAR(250) NULL,
	telefono VARCHAR(30) NOT NULL,
	ciudad VARCHAR(10) NOT NULL,
	banco VARCHAR(20) NOT NULL,
	cuentaBancaria DECIMAL(15) NOT NULL DEFAULT 0,
	usuarioRegistro VARCHAR(100) NULL DEFAULT SUSER_NAME(),
	registroActivo BIT NULL DEFAULT 1,
	fechaRegistro DATETIME NULL DEFAULT GETDATE()
);
CREATE TABLE Empleado(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	cedulaIdentidad VARCHAR(15) NOT NULL,
	nombre VARCHAR(30) NOT NULL,
	primerApellido VARCHAR(30) NOT NULL,
	segundoApellido VARCHAR(30) NOT NULL,
	direccion VARCHAR(250) NOT NULL,
	celular BIGINT NOT NULL,
	cargo VARCHAR(50) NOT NULL,
	usuarioRegistro VARCHAR(100) NULL DEFAULT SUSER_NAME(),
	registroActivo BIT NULL DEFAULT 1,
	fechaRegistro DATETIME NULL DEFAULT GETDATE()
);
CREATE TABLE Cliente(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	nit BIGINT NOT NULL,
	razonSocial VARCHAR(100) NOT NULL,
	telefono VARCHAR(30) NOT NULL,
	usuarioRegistro VARCHAR(100) NULL DEFAULT SUSER_NAME(),
	registroActivo BIT NULL DEFAULT 1,
	fechaRegistro DATETIME NULL DEFAULT GETDATE()
);
CREATE TABLE Usuario(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	idEmpleado INT NOT NULL,
	usuario VARCHAR(12)NOT NULL,
	clave VARCHAR(250) NOT NULL,
	usuarioRegistro VARCHAR(100) NULL DEFAULT SUSER_NAME(),
	registroActivo BIT NULL DEFAULT 1,
	fechaRegistro DATETIME NULL DEFAULT GETDATE()
	CONSTRAINT fk_Usuario_Empleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(id)
);
CREATE TABLE Compra(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	idProveedor INT NOT NULL,
	transaccion INT NOT NULL,
	fecha DATE NOT NULL DEFAULT GETDATE(),
	CONSTRAINT fk_Compra_Proveedor FOREIGN KEY (idProveedor) REFERENCES Proveedor(id)
);
CREATE TABLE CompraDetalle(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	idCompra INT NOT NULL,
	idProducto INT NOT NULL,
	cantidad DECIMAL NOT NULL CHECK (cantidad > 0),
	precioUnitario DECIMAL NOT NULL,
	total DECIMAL NOT NULL
	CONSTRAINT fk_CompraDetalle_Compra FOREIGN KEY (idCompra) REFERENCES Compra(id),
	CONSTRAINT fk_CompraDetalle_Producto FOREIGN KEY (idProducto) REFERENCES Producto(id)
);
CREATE TABLE Venta(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	idUsuario INT NOT NULL,
	idCliente INT NOT NULL,
	transaccion INT NOT NULL,
	fecha DATE NOT NULL DEFAULT GETDATE(),
	CONSTRAINT fk_Venta_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(id),
	CONSTRAINT fk_Venta_Cliente FOREIGN KEY (idCliente) REFERENCES Cliente(id)
);
CREATE TABLE VentaDetalle(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	idVenta INT NOT NULL,
	idProducto INT NOT NULL,
	cantidad DECIMAL NOT NULL CHECK (cantidad > 0),
	precioUnitario DECIMAL NOT NULL,
	total DECIMAL NOT NULL,
	CONSTRAINT fk_VentaDetalle_Venta FOREIGN KEY (idVenta) REFERENCES Venta(id),
	CONSTRAINT fk_VentaDetalle_Producto FOREIGN KEY (idProducto) REFERENCES Producto(id)
);
-- DML
INSERT INTO Producto(codigo, descripcion, unidadMedida, existencias, marca, precioVenta)
VALUES ('BR028', 'Buje de bronze de 28mm', 'unidad', 15, 'Brasilero', 37);

SELECT * FROM Producto;

CREATE PROC paProductoListar @parametro VARCHAR(50)
AS
  SELECT id, codigo, descripcion, unidadMedida, existencias, marca, precioVenta,
		 usuarioRegistro, fechaRegistro
  FROM Producto
  WHERE registroActivo=1 AND 
		codigo+descripcion+unidadMedida LIKE '%'+REPLACE(@parametro,' ','%')+'%'

EXEC paProductoListar ''

CREATE PROC paClienteListar @parametro VARCHAR(50)
AS
	SELECT id, nit, razonSocial, usuarioRegistro, fechaRegistro 
	FROM Cliente
	WHERE registroActivo = 1 AND
			convert(varchar(15),nit)+razonSocial LIKE '%'+REPLACE(@parametro,' ','%')+'%';

EXEC paClienteListar ''

CREATE PROC paUsuarioListar @parametro VARCHAR(50)
AS
  SELECT u.idEmpleado, u.id as idUsuario, e.cedulaIdentidad, e.nombre,
		 e.primerApellido, e.segundoApellido, e.direccion, e.celular,
		 e.cargo, u.usuario, u.usuarioRegistro, u.fechaRegistro
  FROM Empleado e
  INNER JOIN Usuario u ON e.id=u.idEmpleado
  WHERE e.registroActivo=1 AND u.registroActivo=1 AND
		e.cedulaIdentidad+e.nombre+e.primerApellido+e.segundoApellido+u.usuario 
			LIKE '%'+REPLACE(@parametro,' ','%')+'%'
			
EXEC paUsuarioListar ''

CREATE PROC paProveedorListar @parametro VARCHAR(50)
AS
	SELECT id, nit, razonSocial, direccion, telefono, ciudad, banco, cuentaBancaria, usuarioRegistro, fechaRegistro 
	FROM Proveedor
	WHERE registroActivo = 1 AND
			convert(varchar(15),nit)+razonSocial LIKE '%'+REPLACE(@parametro,' ','%')+'%';

EXEC paProveedorListar ''
