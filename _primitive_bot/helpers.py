import math


def direction(point1, point2):
    try:
        tg = (point2['z'] - point1['z']) / (point2['x'] - point1['x'])
        a = math.atan(tg)
    except ZeroDivisionError:
        return 0
    return a * 180 / math.pi

