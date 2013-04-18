TreasureGenerator.exe: TreasureGenerator.cs program.cs
	gmcs -debug *.cs
TreasureGenerator.exe.mdb: TreasureGenerator.exe
run: TreasureGenerator.exe TreasureGenerator.exe.mdb
	mono --debug TreasureGenerator.exe
clean:
	rm *.exe
	rm *.mdb
