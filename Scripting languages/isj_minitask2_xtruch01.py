# minitask 2
# change the last du to DU
import re
pattern = re.compile(r'ud')

text = ['du du du', 'du po ledu', 'dopĹedu du', 'i dozadu du', 'dudu dupl', 'Rammstein du hast']
for row in text:
    print(re.sub(pattern, 'UD', row[::-1],1)[::-1])

