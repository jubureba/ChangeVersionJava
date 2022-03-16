@echo off
   goto SelecionarJava
   :SelecionarJava
   cls

   echo ---------------------------
   echo [101;93m Change your JAVA version[0m 
   echo [7m  run as administrator   [0m   
   echo ---------------------------

   echo Choose the Version you want to use:
   echo [31m[1] - Java 7[0m
   echo [31m[2] - Java 8[0m
   echo [31m[3] - Java 11[0m
   echo [31m[4] - Java 14[0m
   echo [31m[5] - Java 17[0m

   set /a one=1
   set /a two=2
   set /a three=3
   set /a four=4
   set /a five=5
   set input=
   set /p input= Enter your choice:
   if %input% equ %one% goto A if NOT goto SelecionarJava
   if %input% equ %two% goto B if NOT goto SelecionarJava
   if %input% equ %three% goto C if NOT goto SelecionarJava
   if %input% equ %four% goto D if NOT goto SelecionarJava
   if %input% equ %five% goto E if NOT goto SelecionarJava
   if %input% geq %five% goto N

   :A
      cls
      echo You have selected Java 7
      set JAVA="C:\Program Files\Java\jdk1.7.0_80"
      echo %JAVA%
      call:changeVersion JAVA
      pause
      exit

   :B
      cls
      echo You have selected Java 8
      set JAVA=""
      echo %JAVA%
      call:changeVersion JAVA
      pause
      exit   

   :C
      cls
      echo You have selected Java 11
      set JAVA="C:\Program Files\Java\jdk-11.0.12"
      echo %JAVA%
      call:changeVersion JAVA
      pause
      exit

   :D
      cls
      echo You have selected Java 14
      set JAVA=""
      echo %JAVA%
      call:changeVersion JAVA
      pause
      exit

   :E
      cls
      echo You have selected Java 17
      set JAVA="C:\Program Files\Java\jdk-17.0.2"
      echo %JAVA%
      call:changeVersion JAVA
      pause
      exit

   :N
      cls
      echo Invalid Selection! Try again
      pause
      goto :SelecionarJava

   :changeVersion
      cls
      setx /M JAVA_HOME %JAVA%
      echo Variable JAVA_HOME change to %JAVA%
      pause
      exit


