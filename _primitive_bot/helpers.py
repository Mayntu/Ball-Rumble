import math


# Возвращает направление в градусах от точки point1 к точке point2
def direction(point1, point2):
    dx = point2['x'] - point1['x']
    dz = point2['z'] - point1['z']
    try:
        a = math.atan2(dz, dx)
    except ZeroDivisionError:
        return 90 if dz > 0 else -90
    return math.degrees(a)


# Возвращает расстояние от точки point1 до точки point2
def distance(point1, point2):
    dx = point2['x'] - point1['x']
    dz = point2['z'] - point1['z']
    return math.sqrt(dx**2 + dz**2)
