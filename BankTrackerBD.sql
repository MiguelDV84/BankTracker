-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         9.6.0 - MySQL Community Server - GPL
-- SO del servidor:              Linux
-- HeidiSQL Versión:             12.15.0.7171
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para BankTracker
CREATE DATABASE IF NOT EXISTS `BankTracker` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `BankTracker`;

-- Volcando estructura para tabla BankTracker.Cuenta
CREATE TABLE IF NOT EXISTS `Cuenta` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `NumeroCuenta` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  `DniCliente` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `NombreTitular` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  `Saldo` decimal(18,2) NOT NULL DEFAULT '0.00',
  `PasswordHash` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Volcando datos para la tabla BankTracker.Cuenta: ~1 rows (aproximadamente)
REPLACE INTO `Cuenta` (`Id`, `NumeroCuenta`, `DniCliente`, `NombreTitular`, `Saldo`, `PasswordHash`) VALUES
	(3, '1001 54 65', '06280146L', 'Alberto Muñoz', 5000.00, '$2a$11$plgUKujp0thGJlw.Uh3Z0O5FvwkMLvA.XhLR6d/rMHbaOMZ89jpse');

-- Volcando estructura para tabla BankTracker.Movimientos
CREATE TABLE IF NOT EXISTS `Movimientos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `cantidad` decimal(18,2) NOT NULL,
  `concepto` varchar(120) DEFAULT NULL,
  `fecha` datetime NOT NULL,
  `TipoMovimiento` tinyint NOT NULL,
  `cuentaId` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `Movimientos_Cuenta_id_fk` (`cuentaId`) USING BTREE,
  CONSTRAINT `FK1_cuenta_movimiento` FOREIGN KEY (`cuentaId`) REFERENCES `Cuenta` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Volcando datos para la tabla BankTracker.Movimientos: ~5 rows (aproximadamente)
REPLACE INTO `Movimientos` (`id`, `cantidad`, `concepto`, `fecha`, `TipoMovimiento`, `cuentaId`) VALUES
	(3, 500.00, 'Ingreso', '2026-03-17 09:04:57', 0, 3),
	(4, 500.00, 'Ingreso', '2026-03-17 09:05:50', 0, 3),
	(5, -500.00, 'Gasto', '2026-03-17 09:06:25', 1, 3),
	(6, -500.00, 'Gasto', '2026-03-17 09:07:25', 1, 3),
	(7, -500.00, 'Gasto', '2026-03-17 09:07:40', 1, 3);

-- Volcando estructura para procedimiento BankTracker.sp_delete_cuenta
DELIMITER //
CREATE PROCEDURE `sp_delete_cuenta`(IN p_id int)
BEGIN
    DELETE FROM Cuenta WHERE id = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_delete_movimiento
DELIMITER //
CREATE PROCEDURE `sp_delete_movimiento`(IN p_id int)
BEGIN
    DELETE FROM Movimientos WHERE id = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_get_cuenta_dni
DELIMITER //
CREATE PROCEDURE `sp_get_cuenta_dni`(
	IN `p_dni_cliente` VARCHAR(120)
)
BEGIN
	SELECT * FROM Cuenta
	WHERE DniCliente = p_dni_cliente;
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_get_cuentas
DELIMITER //
CREATE PROCEDURE `sp_get_cuentas`()
BEGIN
    SELECT id, numero_cuenta, nombre_titular, saldo, password_hash FROM Cuenta;
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_get_movimientos
DELIMITER //
CREATE PROCEDURE `sp_get_movimientos`()
BEGIN
    SELECT id, cantidad, concepto, fecha, TipoMovimiento, cuentaId FROM Movimientos;
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_insert_cuenta
DELIMITER //
CREATE PROCEDURE `sp_insert_cuenta`(
	IN `p_numero_cuenta` varchar(120),
	IN `p_nombre_titular` varchar(120),
	IN `p_saldo` decimal(18, 2),
	IN `p_password_hash` varchar(255),
	IN `p_dni_cliente` VARCHAR(50)
)
BEGIN
    INSERT INTO Cuenta (NumeroCuenta, NombreTitular,DniCliente, Saldo, PasswordHash)
    VALUES (p_numero_cuenta, p_nombre_titular, p_dni_cliente, p_saldo, p_password_hash);
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_insert_movimiento
DELIMITER //
CREATE PROCEDURE `sp_insert_movimiento`(
	IN `p_cantidad` decimal(18, 2),
	IN `p_concepto` varchar(120),
	IN `p_fecha` datetime,
	IN `p_tipo_movimiento` tinyint,
	IN `p_cuenta_id` int
)
BEGIN
    INSERT INTO Movimientos (cantidad, concepto, fecha, TipoMovimiento, cuentaId)
    VALUES (p_cantidad, p_concepto, p_fecha, p_tipo_movimiento, p_cuenta_id);
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_update_cuenta
DELIMITER //
CREATE PROCEDURE `sp_update_cuenta`(
	IN `p_id` int,
	IN `p_numero_cuenta` varchar(120),
	IN `p_nombre_titular` varchar(120),
	IN `p_saldo` decimal(18, 2),
	IN `p_password_hash` varchar(255)
)
BEGIN
    UPDATE Cuenta
    SET NumeroCuenta = p_numero_cuenta,
        NombreTitular = p_nombre_titular,
        Saldo = p_saldo,
        PasswordHash = p_password_hash
    WHERE Id = p_id;
END//
DELIMITER ;

-- Volcando estructura para procedimiento BankTracker.sp_update_movimiento
DELIMITER //
CREATE PROCEDURE `sp_update_movimiento`(IN p_id int, IN p_cantidad decimal(18, 2), IN p_concepto varchar(120),
                                                         IN p_fecha datetime, IN p_tipo_movimiento tinyint, IN p_cuenta_id int)
BEGIN
    UPDATE Movimientos
    SET cantidad = p_cantidad,
        concepto = p_concepto,
        fecha = p_fecha,
        tipo_movimiento = p_tipo_movimiento,
        cuenta_id = p_cuenta_id
    WHERE id = p_id;
END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
