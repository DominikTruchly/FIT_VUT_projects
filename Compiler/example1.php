<?php
declare(strict_types=1);
// Hello World example i  n IFJ22
// run it on Merlin by: php8.1 ifj22.php hello.php


function hlavni_program(int $year) : void {
  write("Hello from IFJ", $year, "\n");
  return;
}

$a=5.0;

hlavni_program($a);

write($a);
