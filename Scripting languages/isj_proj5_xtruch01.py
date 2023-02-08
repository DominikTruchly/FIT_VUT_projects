def gen_quiz(qpool, *indexes, altcodes = 'ABCDEF', quiz = None):
    if not quiz:
        quiz = []
    for index in indexes:
        try:
            toAdd = qpool[index]
        except IndexError as raised:
            print('Ignoring index ' + str(index) + ' - ' + str(raised)) 
            
        else:
            answersList = []
            for index, answer in enumerate(toAdd[1]):
                string = altcodes[index] + ": " + answer
                answersList.append(string)
            add = (toAdd[0], answersList)
            quiz.append(add)
    return quiz
    

# test_qpool1 = [('Question1', ['Answer1', 'Answer2', 'Answer3', 'Answer4']), ('Question2', ['Answer1', 'Answer2', 'Answer3']), ('Question3', ['Answer1', 'Answer2', 'Answer3', 'Answer4']), ('Question4', ['Answer1', 'Answer2'])]
# existing_quiz1 = [('ExistingQuestion1', ['1: Answer1', '2: Answer2', '3: Answer3']), ('ExistingQuestion2', ['1: Answer1', '2: Answer2'])]
# gen_quiz(test_qpool1, -2, 0, altcodes = ('10', '20', '30'), quiz = existing_quiz1)
# print("[('ExistingQuestion1', ['1: Answer1', '2: Answer2', '3: Answer3']), ('ExistingQuestion2', ['1: Answer1', '2: Answer2']), ('Question3', ['10: Answer1', '20: Answer2', '30: Answer3']), ('Question1', ['10: Answer1', '20: Answer2', '30: Answer3'])]")
# print("-------------------------")
# print(existing_quiz1)
