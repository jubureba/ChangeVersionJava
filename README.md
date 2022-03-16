Change Java Version
------------- 

- Mude a versão do jdk rapidamente.
- Faça o cadastro dos path da JDK instaladas no seu sistema, e troque a versão a hora que quiser, através do `Combobox`  localizado na tela inicial, ou do `TrayMenu` ao lado do relogio. 
- Matenha a aplicação rodando em backgroud ativando nas configurações o `Slider Bar` para trocar rapidamente a hora que quiser. 

`Download` : <https://github.com/jubureba/ChangeVersionJava/releases>
                    
Tela Inicial 
------------- 
[![tela inicial](https://user-images.githubusercontent.com/17513057/157051904-b9ec50fc-e7a7-4c44-8fe9-ffae36c7e3c5.png "tela inicial")](https://user-images.githubusercontent.com/17513057/157051904-b9ec50fc-e7a7-4c44-8fe9-ffae36c7e3c5.png "tela inicial")  

TrayMenu
------------- 
[![traymenu](https://user-images.githubusercontent.com/17513057/157052095-85fa533d-d294-4757-ab8a-ba8a83ae15df.png "traymenu")](https://user-images.githubusercontent.com/17513057/157052095-85fa533d-d294-4757-ab8a-ba8a83ae15df.png "traymenu")  


A ideia
------------- 
A idealização dessa aplicação saiu da necessidade de troca do JDK na variável de ambiente no SO Windows.
Uma vez que você trabalha com Java 11 por exemplo, e troca para o 17 para estudar novas features, se torna trabalhoso e as pessoas buscam ferramentas que fazem isso, indepentende se você já tem uma forma de fazer-lo, e não o tinha... dai surgiu a ideia, que primeiro foi scriptado em .bat: 

[![script em batch](https://user-images.githubusercontent.com/17513057/158603076-7cda7ee8-dfca-43be-8dcf-6e4bf00e96a6.png)

Esta era eficiente para mim, porem a ideia que houvessem usuários que eu pudesse ajudar, o script se tornara complicado. Uma vez que teria que ser editado com o path do jdk de cada usuario, e adição das versões utilizadas:
```javascript
	if %input% equ %one% goto A if NOT goto SelecionarJava
	if %input% geq %five% goto N
	:A
		cls
		echo You have selected Java 7
		set JAVA="C:\Program Files\Java\jdk1.7.0_80"
		echo %JAVA%
		call:changeVersion JAVA
	pause
	exit
```
Pensando nisso, resolvi transferir o batch para uma aplicação onde o usuario só precisasse definir o path, e então poderia trocar rapidamente a versão. 

ps: A versão em .bat se encontra na aplicação para quem quiser ver:
https://github.com/jubureba/ChangeVersionJava/tree/main/ChangeJavaVersion/Batch%20Reference
