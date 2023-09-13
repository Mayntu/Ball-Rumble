#!/usr/bin/python3
# -*- coding: utf-8 -*-
import socket
import json
import math


def direction(point1, point2):
	print(point1, point2)
	try:
		tg = (point2['z'] - point1['z']) / (point2['x'] - point1['x'])
		a = math.atan(tg)
	except ZeroDivisionError:
		return 0
	return a * 180 / math.pi


def get_ball(object_list):
	for x in object_list:
		if x['tag'].lower() == 'ball':
			return x


def get_action_list(rec_obj):
	actions = {'data': []}
	try:
		objects = rec_obj['actions']['data']
	except KeyError:
		return {'error': 1}
	ball = get_ball(objects)
	units = filter(lambda x: 'player' in x['tag'].lower(), objects)
	for u in units:
		dir_to_ball = direction(u['position'], ball['position'])
		act = {'id': u['id'], 'type': 'run', 'force': 50, 'direction': dir_to_ball, 'angle': 0}
		actions['data'].append(act)
	return actions


port_binded = False
udp_port = 8201
server_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

while not port_binded:
	try:
		server_socket.bind(("", udp_port))
		port_binded = True
	except OSError:
		udp_port += 1


print("UDPServer Waiting for client on UDP port ", udp_port)
string_to_send = ''


while 1:
	data, address = server_socket.recvfrom(4096)   # receive data and sender's address
	
	try:
		received_string = str(object=data, encoding='utf-8')
		received_object = json.loads(received_string)
	except ValueError:
		received_string = ""
		received_object = {}

	if received_object:
		print("\nHave received an object: ", received_object)
		string_to_send = json.dumps(get_action_list(received_object))
	else:
		print("\nHave received a message: ", received_string)
		string_to_send = "String OK" if received_string else "Error"
	
	server_socket.sendto(bytes(string_to_send, 'utf-8'), address)

