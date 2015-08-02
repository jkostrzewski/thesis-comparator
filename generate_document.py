import sys
import random
import datetime
import uuid

def get_word(length):
	alphabet = ['a', 'b', 'c', 'd']
	word = ''.join([random.choice(alphabet) for i in xrange(random.randrange(1,length))])
	return word

def main(no_words, word_length):
	doc_list = []
	for wi in xrange(no_words):
		doc_list.append(get_word(word_length))
	doc=' '.join(doc_list)

	filename = uuid.uuid4()
	f = open('test-documents/{}.txt'.format(filename), 'w+')
	f.write(doc)
	f.close()


if __name__=='__main__':
	main(int(sys.argv[1]), int(sys.argv[2]))