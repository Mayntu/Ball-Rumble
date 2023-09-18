import socket
import json


class GameConnector:
    PORT = 8201

    def __init__(self, logic_function: callable(list)):
        self.process = logic_function
        self.port = self.PORT
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.started = False
        bind_ok = False
        while not bind_ok:
            try:
                self.socket.bind(("", self.port))
                bind_ok = True
            except OSError:
                self.port += 1

    def start(self):
        print("Waiting for game on UDP port ", self.port)
        self.started = True
        while self.started:
            request = {}
            response_data = []
            data, address = self.socket.recvfrom(4096)
            try:
                request = json.loads(str(object=data, encoding='utf-8'))
            except ValueError:
                pass

            print("\nGot request: ", request)
            if 'actions' in request:
                response_data = self.process(request['actions']['data'])

            response_str = json.dumps({'data': response_data})
            self.socket.sendto(bytes(response_str, 'utf-8'), address)

    def stop(self):
        self.started = False
        self.socket.close()
