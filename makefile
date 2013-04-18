GemGenerator.exe: GemGenerator.cs program.cs
	gmcs -debug *.cs
GemGenerator.exe.mdb: GemGenerator.exe
run: GemGenerator.exe GemGenerator.exe.mdb
	mono --debug GemGenerator.exe
clean:
	rm *.exe
	rm *.mdb
