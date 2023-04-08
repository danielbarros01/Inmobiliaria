CREATE DATABASE  IF NOT EXISTS `inmobiliaria` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `inmobiliaria`;
-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: inmobiliaria
-- ------------------------------------------------------
-- Server version	8.0.25

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `contratos`
--

DROP TABLE IF EXISTS `contratos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contratos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Desde` datetime NOT NULL,
  `Hasta` datetime NOT NULL,
  `Condiciones` varchar(255) NOT NULL,
  `Monto` decimal(10,0) NOT NULL,
  `inmueble_Id` int NOT NULL,
  `inquilino_Id` int NOT NULL,
  PRIMARY KEY (`Id`,`inmueble_Id`,`inquilino_Id`),
  KEY `fk_contratos_inmuebles1_idx` (`inmueble_Id`),
  KEY `fk_contratos_inquilinos1_idx` (`inquilino_Id`),
  CONSTRAINT `fk_contratos_inmuebles1` FOREIGN KEY (`inmueble_Id`) REFERENCES `inmuebles` (`Id`),
  CONSTRAINT `fk_contratos_inquilinos1` FOREIGN KEY (`inquilino_Id`) REFERENCES `inquilinos` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contratos`
--

LOCK TABLES `contratos` WRITE;
/*!40000 ALTER TABLE `contratos` DISABLE KEYS */;
INSERT INTO `contratos` VALUES (16,'2000-01-01 00:00:00','2023-07-04 00:00:00','Muchas',58000,2,4),(19,'2023-04-15 18:00:00','2023-04-14 18:00:00','No se permiten niños, un garante y mes de deposito',85000,3,4);
/*!40000 ALTER TABLE `contratos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inmuebles`
--

DROP TABLE IF EXISTS `inmuebles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmuebles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(45) NOT NULL,
  `Uso` varchar(45) NOT NULL,
  `Cantidad_ambientes` int NOT NULL,
  `Coordenadas` varchar(45) NOT NULL,
  `Precio` decimal(9,2) NOT NULL,
  `Disponible` tinyint NOT NULL,
  `propietario_Id` int NOT NULL,
  `tipo_inmueble_Id` int NOT NULL,
  PRIMARY KEY (`Id`,`propietario_Id`,`tipo_inmueble_Id`),
  KEY `fk_inmuebles_propietarios_idx` (`propietario_Id`),
  KEY `fk_inmuebles_tipos_inmueble1_idx` (`tipo_inmueble_Id`),
  CONSTRAINT `fk_inmuebles_propietarios` FOREIGN KEY (`propietario_Id`) REFERENCES `propietarios` (`Id`),
  CONSTRAINT `fk_inmuebles_tipos_inmueble1` FOREIGN KEY (`tipo_inmueble_Id`) REFERENCES `tipos_inmueble` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmuebles`
--

LOCK TABLES `inmuebles` WRITE;
/*!40000 ALTER TABLE `inmuebles` DISABLE KEYS */;
INSERT INTO `inmuebles` VALUES (2,'Ruta 1','Residencial',4,'XX210, 23AS1',75000.00,1,8,2),(3,'San Martin 799','Residencial',7,'BB6650, 645213FADS',110000.00,1,7,1),(4,'Belgrano Sur 992','Residencial',5,'Bxadas50, 64521332FADS',550.00,1,6,8);
/*!40000 ALTER TABLE `inmuebles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inquilinos`
--

DROP TABLE IF EXISTS `inquilinos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilinos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Dni` varchar(45) NOT NULL,
  `Apellido` varchar(45) NOT NULL,
  `Nombre` varchar(45) NOT NULL,
  `Email` varchar(45) DEFAULT NULL,
  `Telefono` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Dni_UNIQUE` (`Dni`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilinos`
--

LOCK TABLES `inquilinos` WRITE;
/*!40000 ALTER TABLE `inquilinos` DISABLE KEYS */;
INSERT INTO `inquilinos` VALUES (4,'88996554','Viedo','Elmiro','Elimir@hotmail.com','26665448745');
/*!40000 ALTER TABLE `inquilinos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietarios`
--

DROP TABLE IF EXISTS `propietarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Dni` varchar(45) NOT NULL,
  `Nombre` varchar(45) NOT NULL,
  `Apellido` varchar(45) NOT NULL,
  `Email` varchar(45) NOT NULL,
  `Telefono` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Dni_UNIQUE` (`Dni`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietarios`
--

LOCK TABLES `propietarios` WRITE;
/*!40000 ALTER TABLE `propietarios` DISABLE KEYS */;
INSERT INTO `propietarios` VALUES (6,'665521','Padre','Lucas','padrelucas@outlook.com','665588777'),(7,'88999774','Ernesto','Rothbard','erobard@hotmail.com','2321321231'),(8,'55555552','dasdsa','Barros','danibelgranocab15@gmail.com','3544562721');
/*!40000 ALTER TABLE `propietarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipos_inmueble`
--

DROP TABLE IF EXISTS `tipos_inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipos_inmueble` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Tipo` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Tipo_UNIQUE` (`Tipo`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipos_inmueble`
--

LOCK TABLES `tipos_inmueble` WRITE;
/*!40000 ALTER TABLE `tipos_inmueble` DISABLE KEYS */;
INSERT INTO `tipos_inmueble` VALUES (2,'Casa'),(1,'Departamento'),(8,'Monoambiente');
/*!40000 ALTER TABLE `tipos_inmueble` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-04-08 18:48:27
