-- uart_fsm.vhd: UART controller - finite state machine
-- Author(s): xtruch01
--
library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
-------------------------------------------------
entity UART_FSM is
port(
   CLK:      in std_logic;
   RST:      in std_logic;
   DIN:      in std_logic;
   MSB_READ: in std_logic;
   
   CLK_CNT:  in std_logic_vector(4 downto 0);
   
   DATA_READ_EN: out std_logic;
   CLK_CNT_EN:   out std_logic;
   DOUT_VLD:     out std_logic
   );
end entity UART_FSM;

-------------------------------------------------
architecture behavioral of UART_FSM is
type STATES is (START, WAIT_t , READ, END_t , VALID);
signal state : STATES := START;
begin
  CLK_CNT_EN <= '0' when state = VALID or state = START else '1';
  DATA_READ_EN <= '1' when state = READ else '0';
  DOUT_VLD <= '1' when state = VALID else '0';
  
  process(CLK) begin
    if rising_edge(CLK) then
      if RST = '1' then
        state <= START;
      else
        case state is
        when START => if DIN = '0' then
                            state <= WAIT_t;
                           end if;
        when WAIT_t => if CLK_CNT = "10110" then
                            state <= READ;
                          end if;
        when READ => if MSB_READ = '1' then
                            state <= END_t; 
                          end if;
        when END_t =>  if CLK_CNT = "10000" then
                            state <= VALID; 
                          end if;
        when VALID => state <= START;
        
        when others => null;
                          
        end case;
    end if;
  end if;
end process;
end behavioral;

