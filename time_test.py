import subprocess
import time
import os
#compareAlgorithms = ['SentenceSplit', 'EquiWidth', 'EquiWidthSkip','WordSplit']
compareAlgorithms = ['WordSplit']
#patternAlgorithms = ['Naive', 'AXAMAC', 'BoyerMoore', 'DeterministicFiniteAutomaton', 'MorrisPratt', 'KMP', 'RabinKarp']
#patternAlgorithms = ['Naive', 'AXAMAC', 'MorrisPratt', 'KMP', 'RabinKarp']
patternAlgorithms = ['Naive']
#noThreads = ['1', '2', '4', '8', '16']
noThreads = ['8']

def main():
	f = open('result.txt', "w+")
	f.close()
	resultTable = [[0 for pa in patternAlgorithms] for ca in compareAlgorithms]
	for ca in compareAlgorithms:
		for pa in patternAlgorithms:
			for nt in noThreads:
				print '{0}\t{1}\t{2}\t'.format(ca, pa, nt),
				begin = time.time()
				with open('result.txt', "a+") as f:
					
					result = subprocess.call([os.path.dirname(__file__)+'\\thesis-comparator\\TestApp1\\bin\\Release\\TestApp1.exe', ca, pa, nt], stdout=False)
				end = time.time()
				print '{0}'.format(end-begin)
				

	





if __name__ == '__main__':
	main()