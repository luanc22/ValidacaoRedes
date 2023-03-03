Para rodar os programas, realize este passo-a-passo:

Server (deve ser aberto primeiro):
UDPServer > bin > Debug > net6.0 > UDPServer.exe

Cliente:
UDPClient > bin > Debug > net6.0 > UDPClient.exe

Vai abrir ambos os terminais, digite os comandos somente no terminal de cliente.
Fiz de maneira que os comandos são digitas em ordem, ou seja, primeiro irá pedir o comando "get" ou "set", depois irá mostrar a lista de locais disponíveis e pedirá para digitar um local, em seguida, caso o comando selecionado seja "set", irá pedir um valor "on" ou "off".

Após isso, ele ira mostrar a resposta do servidor, caso queira continuar utilizando o programa, digite qualquer tecla, caso queira sair, digite "0"