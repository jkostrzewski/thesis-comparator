@echo off
setlocal
set "compare=NGramSplit, SentenceSplit, EquiWidth, EquiWidthSkip"
set "pattern=Naive"
set "threads=8, 4, 2, 1"

 for %%c in (%compare%) do (
 	for %%p in (%pattern%) do (
 		for %%t in (%threads%) do (
 			thesis-comparator\TestApp1\bin\Release\TestApp1.exe %%c %%p %%t
 		)
 	)
 )
  