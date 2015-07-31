# -*- coding: utf-8 -*-
import sys
sys.path.append('C:\Python27\Lib')
sys.path.append('C:\Python27\Lib\site-packages')
from jinja2 import Template
import json
import os
import sys
#import webbrowser

def main(in_file, output):
	reload(sys)

	sys.setdefaultencoding('utf-8')
	template = Template(open(os.path.dirname(__file__)+'/templates/report_template.html', 'r').read())
	print "loading json data"
	data_in = json.loads(open(in_file, 'r').read())
	#data['userText']['text'] = prepare_user_text(data)
	data = prepare_data(data_in)

	output_file = open(output, 'w+')
	print "rendering template"
	output_file.write(template.render(data=data))
	output_file.close()
	os.startfile(output)

def prepare_data(data_in):
	print "preparing data to render"
	data = preprocess_data(data_in)
	data["statistics"] = {}
	data["statistics"]['main'] = {}
	data["statistics"]['main']['userDocLength'] = len(data['userText']['text'])
	for e in sorted(data['matched'], key=lambda x:x['us'], reverse=True):
		t = data['userText']['text']
		data['userText']['text'] = t[:e['us']]+'<span class="match" title="{0}" docId="{1}" us="{2}" ue="{3}" ds="{4}" de="{5}">'.format(get_name(data, e['id']), e['id'], e['us'], e['ue'], e['ds'], e['de'])+t[e['us']:e['ue']]+'</span>'+t[e['ue']:]
	data['meta']['time'] = float(data['meta']['time'].replace(',','.'))
	data['statistics']['match'] = get_match_statistics(data)
	data["statistics"]['main']['overall'] = float(sum(data['statistics']['match'].values()))*100/data["statistics"]['main']['userDocLength']
	
	return data

def preprocess_data(data):
	matched = []
	booked = [False]*(len(data['userText']['text'])+1)
	print len(booked)
	for e in sorted(data['matched'], key=lambda x:x['ue']-x['us'], reverse=True):
		if booked[e['us']] is False and booked[e['ue']] is False:
			for i in xrange(e['us'], e['ue']+1):
				booked[i] = True
			matched.append(e)
	data['matched'] = matched
	return data

def get_match_statistics(data):
	stats = {}
	for e in data['matched']:
		if get_name(data, e['id']) not in stats:
			stats[get_name(data, e['id'])] = 0
		stats[get_name(data, e['id'])] += (e['ue'] - e['us'])
	return stats

def get_name(data, i):
	return data["id_mapping"][i]

if __name__ == '__main__':
	in_file = sys.argv[0]
	output = sys.argv[1]
	main(in_file, output)