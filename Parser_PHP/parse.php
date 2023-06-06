<?php
ini_set("display_errors", "std_err");

function type_check($split, $a){
    if (preg_match("/^((int)|(bool)|(string)){1}$/", $split[$a]) == 1){
        echo "<arg$a type=\"type\">$split[$a]</arg$a>\n";
    }
    else{
        echo "Wrong type\n";
        exit(23); 
    }

}
function label_check($split,$a){
    if(preg_match("/^([a-zA-Z0-9]|_|-|\\\$|&|%|\*|!|\?)+$/", $split[$a]) == 0){
        echo "Wrong label format\n";
        exit(23);
    }
}
function not_variable_check($split, $a){
    if (preg_match("/(GF|LF|TF)@([a-zA-Z]|_|-|\\\$|&|%|\*|!|\?)([a-zA-Z0-9]|_|-|\\\$|&|%|\*|!|\?)*/", $split[1]) == 0){
        echo "Wrong variable format\n";
        exit(23);
    }
}
function regex_check($split, $a) {
    if (preg_match("/^(GF|LF|TF)@([a-zA-Z]|_|-|\\\$|&|%|\*|!|\?)([a-zA-Z0-9]|_|-|\\\$|&|%|\*|!|\?)*/", $split[$a]) == 1){
        $split[$a] = htmlspecialchars($split[$a]);
        echo "<arg$a type=\"var\">$split[$a]</arg$a>\n";
    }
    
    else if (preg_match("/string@((\\\\[0-9]{3})|[^\\\\\s#])*$/", $split[$a]) == 1){
        $type_value = explode("@", $split[$a], 2);    
        $type = $type_value[0];
        $value = $type_value[1];
        $value = htmlspecialchars($value);
        echo "<arg$a type=\"$type\">$value</arg$a>\n";
    }
    else if (preg_match("/int@[-+]?[0-9]+$/", $split[$a]) == 1){
        $type_value = explode("@", $split[$a], 2);    
        $type = $type_value[0];
        $value = $type_value[1];
        echo "<arg$a type=\"$type\">$value</arg$a>\n";
    }
    else if (preg_match("/bool@(true|false)$/", $split[$a]) == 1){
        $type_value = explode("@", $split[$a], 2);    
        $type = $type_value[0];
        $value = $type_value[1];
        echo "<arg$a type=\"$type\">$value</arg$a>\n";
    }
    else if (preg_match("/nil@nil$/", $split[$a]) == 1){
        $type_value = explode("@", $split[$a], 2);    
        $type = $type_value[0];
        $value = $type_value[1];
        echo "<arg$a type=\"$type\">$value</arg$a>\n";
    }
    else{
        echo "Wrong variable format\n";
        exit(23);
    }
}
function argumets_count($split, $number){
    if (count($split) != $number){
        echo "Wrong number of arguments\n";
        exit(23);
    }
}

if ($argc > 1) {
    if ($argc > 2){
        echo "Wrong number of arguments\n";
        exit(10);
    } 
    if ($argv[1] == "--help"){
        echo "parse.php takes IPPcode23 source code and prints it into stdout xml format\n";
        echo "usage: php parse.php [options] < [FILE]\n";
        echo "options:\n";
        echo "        --help: prints help\n";
        exit(0);
    }
}

$header = false;
$order = 0;
while($line = fgets(STDIN)){
    if ($line == "\r\n" || $line == "\n"){
        continue;
    }
    $line = trim($line);
    if ($line[0] == "#"){
        continue;
    }
    $hashtagSplit = explode("#", $line);
    $hashtagSplit[0] = trim($hashtagSplit[0]);
    if ($header == false){
        if ($hashtagSplit[0] == ""){continue;}
        if (strcmp($hashtagSplit[0],".IPPcode23") == 0){
            $header = true;
            echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
            echo "<program language=\"IPPcode23\">\n";
            continue;
        }
        else{
            echo "Wrong or missing header\n";
            exit(21);
        }
    }

    $split = preg_split('/\s+/', $hashtagSplit[0]);
    $func = strtoupper($split[0]);


    $order += 1;
    switch($func){
        case "MOVE":
            argumets_count($split, 3);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"MOVE\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            echo "</instruction>\n";
            break;

        case "CREATEFRAME":
            argumets_count($split, 1);
            echo "<instruction order= \"$order\" opcode=\"CREATEFRAME\">\n";
            echo "</instruction>\n";
            break;

        case "PUSHFRAME":
            argumets_count($split, 1);
            echo "<instruction order= \"$order\" opcode=\"PUSHFRAME\">\n";
            echo "</instruction>\n";
            break;

        case "POPFRAME":
            argumets_count($split, 1);
            echo "<instruction order= \"$order\" opcode=\"POPFRAME\">\n";
            echo "</instruction>\n";
            break;

        case "DEFVAR":
            argumets_count($split, 2);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"DEFVAR\">\n";
            $split[1] = htmlspecialchars($split[1]);
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            echo "</instruction>\n";
            break;
                
        case "CALL":
            argumets_count($split, 2);
            label_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"CALL\">\n";
            echo "<arg1 type=\"label\">$split[1]</arg1>\n";
            echo "</instruction>\n";
            break;

        case "RETURN":
            argumets_count($split, 1);
            echo "<instruction order= \"$order\" opcode=\"RETURN\">\n";
            echo "</instruction>\n";
            break;
        
        case "PUSHS":
            argumets_count($split, 2);
            echo "<instruction order= \"$order\" opcode=\"PUSHS\">\n";
            regex_check($split, 1);
            echo "</instruction>\n";
            break;

        case "POPS":
            argumets_count($split, 2);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"POPS\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            echo "</instruction>\n";
            break;
        
        case "ADD":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"ADD\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "SUB":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"SUB\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "MUL":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"MUL\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "IDIV":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"IDIV\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "LT":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"LT\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "GT":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"GT\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;
        case "EQ":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"EQ\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "AND":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"AND\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;
        case "OR":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"OR\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;
        case "NOT":
            argumets_count($split, 3);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"NOT\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            echo "</instruction>\n";
            break;

        case "INT2CHAR":
            argumets_count($split, 3);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"INT2CHAR\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            echo "</instruction>\n";
            break;

        case "STRI2INT":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"STR2INT\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "READ":
            argumets_count($split, 3);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"READ\">\n";
            type_check($split, 2);
            echo "</instruction>\n";
            break;

        case "WRITE":
            argumets_count($split, 2);
            echo "<instruction order= \"$order\" opcode=\"WRITE\">\n";
            regex_check($split, 1);
            echo "</instruction>\n";
            break;

        case "CONCAT":
            argumets_count($split, 4);
            not_variable_check($split, 2);
            echo "<instruction order= \"$order\" opcode=\"CONCAT\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "STRLEN":
            argumets_count($split, 3);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"STRLEN\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            echo "</instruction>\n";
            break;

        case "GETCHAR":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"GETCHAR\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "SETCHAR":
            argumets_count($split, 4);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"SETCHAR\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "TYPE":
            argumets_count($split, 3);
            not_variable_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"TYPE\">\n";
            echo "<arg1 type=\"var\">$split[1]</arg1>\n";
            regex_check($split, 2);
            echo "</instruction>\n";
            break;
        case "LABEL":
            argumets_count($split, 2);
            label_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"LABEL\">\n";
            echo "<arg1 type=\"label\">$split[1]</arg1>\n";
            echo "</instruction>\n";
            break;
        
        case "JUMP":
            argumets_count($split, 2);
            label_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"JUMP\">\n";
            echo "<arg1 type=\"label\">$split[1]</arg1>\n";
            echo "</instruction>\n";
            break;

        case "JUMPIFEQ":
            argumets_count($split, 4);
            label_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"JUMPIFEQ\">\n";
            echo "<arg1 type=\"label\">$split[1]</arg1>\n";

            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;
        
        case "JUMPIFNEQ":
            argumets_count($split, 4);
            label_check($split, 1);
            echo "<instruction order= \"$order\" opcode=\"JUMPIFNEQ\">\n";
            echo "<arg1 type=\"label\">$split[1]</arg1>\n";
            regex_check($split, 2);
            regex_check($split, 3);
            echo "</instruction>\n";
            break;

        case "EXIT":
            argumets_count($split, 2);
            echo "<instruction order= \"$order\" opcode=\"EXIT\">\n";
            regex_check($split, 1);
            echo "</instruction>\n";
            break;

        case "DPRINT":
            argumets_count($split, 2);
            echo "<instruction order= \"$order\" opcode=\"DPRINT\">\n";
            regex_check($split, 1);
            echo "</instruction>\n";
            break;

        case "BREAK":
            argumets_count($split, 1);
            echo "<instruction order= \"$order\" opcode=\"BREAK\">\n";
            echo "</instruction>\n";
            break;
        case ".IPPCODE23":
            echo("Double header\n");
            exit(22);
            break;

        default:
            echo "Wrong instruction name\n";
            exit(22);
    }
}
echo "</program>\n";
exit(0);

?>
