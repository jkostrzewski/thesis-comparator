import sys
import random
import datetime
import uuid

def get_word(length):
	alphabet = ['a', 'b', 'c', 'd']
	word = ''.join([random.choice(alphabet) for i in xrange(random.randrange(1,length))])
	return word

def main(no_characters):
	alphabet = ['a', 'a', 'a', 'b', 'b', 'b', 'c', 'c', 'c', '. ', ' ']
	doc=''.join([random.choice(alphabet) for i in xrange(no_characters)])

	filename = str(uuid.uuid4())+"_"+str(no_characters)
	f = open('test-documents/{}.txt'.format(filename), 'w+')
	f.write(doc)
	f.close()


if __name__=='__main__':
	main(int(sys.argv[1]))