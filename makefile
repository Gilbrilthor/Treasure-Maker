tgen.exe: TreasureGenerator.cs program.cs ConsoleHelper.dll
	gmcs -debug -reference:ConsoleHelper.dll TreasureGenerator.cs program.cs -out:tgen.exe
tgen.exe.mdb: tgen.exe
run: tgen.exe tgen.exe.mdb
	mono --debug tgen.exe
ConsoleHelper.dll: ConsoleHelper.cs
	gmcs -debug -target:library ConsoleHelper.cs
clean:
	rm *.exe
	rm *.mdb
	rm *.dll
