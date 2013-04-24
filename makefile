tgp.exe: TreasureGenerator.cs program.cs ConsoleMenu.cs ConsoleHelper.dll
	gmcs -debug -reference:ConsoleHelper.dll ConsoleMenu.cs TreasureGenerator.cs program.cs -out:tgp.exe
tgp.exe.mdb: tgp.exe
run: tgp.exe tgp.exe.mdb
	mono --debug tgp.exe
ConsoleHelper.dll: ConsoleHelper.cs
	gmcs -debug -target:library ConsoleHelper.cs
clean:
	rm *.exe
	rm *.mdb
	rm *.dll
