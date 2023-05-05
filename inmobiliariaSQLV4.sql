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
  `Condiciones` varchar(500) NOT NULL,
  `Monto` decimal(10,0) NOT NULL,
  `inmueble_Id` int NOT NULL,
  `inquilino_Id` int NOT NULL,
  PRIMARY KEY (`Id`,`inmueble_Id`,`inquilino_Id`),
  KEY `fk_contratos_inmuebles1_idx` (`inmueble_Id`),
  KEY `fk_contratos_inquilinos1_idx` (`inquilino_Id`),
  CONSTRAINT `fk_contratos_inmuebles1` FOREIGN KEY (`inmueble_Id`) REFERENCES `inmuebles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `fk_contratos_inquilinos1` FOREIGN KEY (`inquilino_Id`) REFERENCES `inquilinos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contratos`
--

LOCK TABLES `contratos` WRITE;
/*!40000 ALTER TABLE `contratos` DISABLE KEYS */;
INSERT INTO `contratos` VALUES (45,'2023-04-26 00:00:00','2023-04-26 15:59:47','dsadsadasdsa',56000,18,9);
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
  `Uso` int NOT NULL,
  `Cantidad_ambientes` int NOT NULL,
  `Coordenadas` varchar(45) NOT NULL,
  `Precio` decimal(9,2) NOT NULL,
  `Disponible` tinyint NOT NULL,
  `propietario_Id` int NOT NULL,
  `tipo_inmueble_Id` int NOT NULL,
  PRIMARY KEY (`Id`,`propietario_Id`,`tipo_inmueble_Id`),
  KEY `fk_inmuebles_propietarios_idx` (`propietario_Id`),
  KEY `fk_inmuebles_tipos_inmueble1_idx` (`tipo_inmueble_Id`),
  CONSTRAINT `fk_inmuebles_propietarios` FOREIGN KEY (`propietario_Id`) REFERENCES `propietarios` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `fk_inmuebles_tipos_inmueble1` FOREIGN KEY (`tipo_inmueble_Id`) REFERENCES `tipos_inmueble` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmuebles`
--

LOCK TABLES `inmuebles` WRITE;
/*!40000 ALTER TABLE `inmuebles` DISABLE KEYS */;
INSERT INTO `inmuebles` VALUES (13,'Bolivar 826',1,4,'-33.297415356218494, -66.33760976961264',105000.00,1,29,2),(14,'Junin 858',1,4,'-33.301297133718606, -66.33602094446354',154000.00,1,29,1),(15,'Belgrano 1100',1,2,'-33.30492924536975, -66.33932718751316',50000.00,1,29,8),(16,'Illia 127',2,5,'-33.30125216065074, -66.33968097872162',200000.00,1,30,2),(17,'Mitre 800',1,2,'-33.30163225945136, -66.34091334037358',70000.00,1,30,8),(18,'Calle 123',1,4,'-33.292996254843764, -66.33644199141078',55000.00,0,31,2);
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilinos`
--

LOCK TABLES `inquilinos` WRITE;
/*!40000 ALTER TABLE `inquilinos` DISABLE KEYS */;
INSERT INTO `inquilinos` VALUES (9,'56254125','SDASD','Ulises','dasdas@hotmail.com','354545521');
/*!40000 ALTER TABLE `inquilinos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pagos`
--

DROP TABLE IF EXISTS `pagos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagos` (
  `NumeroPago` int NOT NULL,
  `Fecha` datetime NOT NULL,
  `contrato_Id` int NOT NULL,
  PRIMARY KEY (`NumeroPago`,`contrato_Id`),
  KEY `fk_pagos_contratos1_idx` (`contrato_Id`),
  CONSTRAINT `fk_pagos_contratos1` FOREIGN KEY (`contrato_Id`) REFERENCES `contratos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pagos`
--

LOCK TABLES `pagos` WRITE;
/*!40000 ALTER TABLE `pagos` DISABLE KEYS */;
INSERT INTO `pagos` VALUES (1,'2023-04-27 00:00:00',45),(2,'2023-04-28 00:00:00',45);
/*!40000 ALTER TABLE `pagos` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietarios`
--

LOCK TABLES `propietarios` WRITE;
/*!40000 ALTER TABLE `propietarios` DISABLE KEYS */;
INSERT INTO `propietarios` VALUES (29,'44855236','Luis','Veliz','luis@gmail.com','366658547'),(30,'47996548','Ernesto','Salas','ernesto@hotmail.com','266457785'),(31,'20111445','Mariano','Luzza','mariano@hotmail.com','266654585');
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

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Rol` int NOT NULL,
  `Email` varchar(45) NOT NULL,
  `Nombre` varchar(45) NOT NULL,
  `Apellido` varchar(45) NOT NULL,
  `Clave` varchar(45) NOT NULL,
  `AvatarRuta` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email_UNIQUE` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (5,2,'dani15@gmail.com','Daniel','Barros','jqHmR/6TBx1wnbguKZWq7l5herWtJ9epvjB5B9tNOiY=','/uploads\\avatar_5.jpg'),(7,1,'juli15@gmail.com','Juli','Veliz','jqHmR/6TBx1wnbguKZWq7l5herWtJ9epvjB5B9tNOiY=','/uploads\\avatar_7.jpg'),(8,2,'eduardo@hotmail.com','Eduardo','Da','jqHmR/6TBx1wnbguKZWq7l5herWtJ9epvjB5B9tNOiY=','/uploads\\avatar_8.jpg');
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-05 13:08:11
