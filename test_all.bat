@echo off
setlocal
set "compare=SentenceSplit,NGramSplit,WordSplit,EquiWidth,EquiWidthSkip"
set "pattern=Naive,KMP,MorrisPratt,RabinKarp"
set "threads=8"

 for %%c in (%compare%) do (
 	for %%p in (%pattern%) do (
 		for %%t in (%threads%) do (
 			thesis-comparator\TestApp1\bin\Release\TestApp1.exe %%c %%p %%t "C:\Users\kuba\Desktop\praca_magisterska\marketing.txt"
 		)
 	)
 )
  