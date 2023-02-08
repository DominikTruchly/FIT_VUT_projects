; Autor reseni: Dominik Truchly xtruch01

; Projekt 2 - INP 2022
; Vernamova sifra na architekture MIPS64

; DATA SEGMENT
                .data
login:          .asciiz "xtruch01"  ; sem doplnte vas login
cipher:         .space  17  ; misto pro zapis sifrovaneho loginu

params_sys5:    .space  8   ; misto pro ulozeni adresy pocatku
                            ; retezce pro vypis pomoci syscall 5
                            ; (viz nize "funkce" print_string)

; CODE SEGMENT
                .text

                ; ZDE NAHRADTE KOD VASIM RESENIM
main:
                addi r5, r0, 96
                addi r23, r0, 123

                ;daddi   r4, r0, login   ; vozrovy vypis: adresa login: do r4

start:
                lb r19, login(r4)           
                slt r1, r5, r19 
                beq r1, r0, end
                addi r19, r19, 20
                slt r1, r19, r23
                beq r1, r0, cycle_below
substraction:
                sb r19, cipher(r4)
                addi r4, r4, 1
                lb r19, login(r4)
                slt r1, r5, r19 
                beq r1, r0, end
                addi  r19, r19, -18
                slt r1, r5, r19
                beq r1, r0,  cycle_over
addition:
                sb r19, cipher(r4)
                addi r4, r4, 1
                j start

cycle_below:
                addi  r19, r19, -26     ; fixes transition below a 
	            j substraction
cycle_over:
                addi r19, r19, 26       ; fixes transition over z
	            j addition

end:
                addi r4, r4, 1
                addi r19, r0, 0
                sb r19, cipher(r4)      ; 
                daddi r4, r0, cipher    ; copy cipher to r4
                jal     print_string ; vypis pomoci print_string - viz nize

                syscall 0

print_string:   ; adresa retezce se ocekava v r4
                sw      r4, params_sys5(r0)
                daddi   r14, r0, params_sys5    ; adr pro syscall 5 musi do r14
                syscall 5   ; systemova procedura - vypis retezce na terminal
                jr      r31 ; return - r31 je urcen na return address

