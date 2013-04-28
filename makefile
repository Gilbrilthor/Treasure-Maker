tgen.exe: TreasureGenerator.cs program.cs ConsoleHelper.dll
	mcs -debug -reference:ConsoleHelper.dll -main:DMTools.Driver TreasureGenerator.cs CoinGenerator.cs program.cs -out:tgen.exe
tgen.exe.mdb: tgen.exe
run: tgen.exe tgen.exe.mdb
	mono --debug tgen.exe
ConsoleHelper.dll: ConsoleHelper.cs
	mcs -debug -target:library ConsoleHelper.cs
clean:
	rm *.exe
	rm *.mdb
	rm *.dll
