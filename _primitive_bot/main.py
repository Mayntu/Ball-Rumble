#!/usr/bin/python3
# -*- coding: utf-8 -*-
from connector import GameConnector
import helpers as hlp


def main_logic(team_name, game_objects):
	actions = []	# нужно сформировать список действий для своих юнитов

	# вытаскиваем игровые объекты:
	ball = tuple(filter(lambda x: x['tag'] == 'Ball', game_objects))[0]			# мяч
	posts = tuple(filter(lambda x: x['tag'] == 'GoalPost', game_objects))		# штанги ворот
	units = tuple(filter(lambda x: x['tag'].endswith('Player'), game_objects))	# все юниты
	my_team = tuple(filter(lambda x: x['tag'] == team_name, units))     		# мои юниты

	for u in my_team:
		if "ball" in u['features']:
			act = {'id': u['id'], 'type': 'run', 'force': 100, 'direction': 0, 'angle': 0}
		else:
			act = {'id': u['id'], 'type': 'run', 'force': 50, 'direction': hlp.direction(u['position'], ball['position']), 'angle': 0}
		actions.append(act)

	return actions 	# возвращаем список действий своих юнитов



if __name__ == '__main__':
	bot = GameConnector(main_logic)
	bot.start()
