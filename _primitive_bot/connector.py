import socket
import json


class GameConnector:
    PORT = 8201

    def __init__(self, logic_function):
        self.process = logic_function
        self.port = self.PORT
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.game_over = False
        self.team_name = ''
        bind_ok = False
        while not bind_ok:
            try:
                self.socket.bind(("", self.port))
                bind_ok = True
            except OSError:
                self.port += 1

    def start(self):
        print("Waiting for game on UDP port ", self.port)
        self.game_over = False
        while not self.game_over:
            request = {}
            response_data = []
            data, address = self.socket.recvfrom(4096)
            try:
                request = json.loads(str(object=data, encoding='utf-8'))
            except ValueError:
                pass
            # print("\nGot request: ", request)

            if 'ready' in request:
                self.team_name = request['ready']['data']['teamName']
                print(f"Team: {self.team_name}")
                response_data = 'ok'
            elif 'actions' in request:
                response_data = self.process(self.team_name, request['actions']['data'])
            elif 'gameOver' in request:
                # TODO: Обязательно раскомментировать перед релизом!
                # self.game_over = True
                response_data = 'ok'

            response_str = json.dumps({'data': response_data})
            self.socket.sendto(bytes(response_str, 'utf-8'), address)
        self.stop()

    def stop(self):
        self.game_over = True
        self.socket.close()
