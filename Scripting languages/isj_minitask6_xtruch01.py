# minitask 6

class Point:
    def __init__(self,x = 0,y = 0):
        self.x = x
        self.y = y
    def __sub__(point1,point2):
        return Point(point1.x-point2.x, point1.y-point2.y)
    def __str__(self):
        return f"Point({self.x}, {self.y})"

p0 = Point()
p1 = Point(3, 4)
print(p0-p1)
p2 = Point(1, 2)
result = p1-p2
print(result)