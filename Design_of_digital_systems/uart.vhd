-- uart.vhd: UART controller - receiving part
-- Author(s): xtruch01
--
library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;

-------------------------------------------------
entity UART_RX is
port(	
  CLK: 	     in std_logic;
	RST: 	     in std_logic;
	DIN: 	     in std_logic;
	DOUT: 	    out std_logic_vector(7 downto 0);
	DOUT_VLD:  out std_logic
);
end UART_RX;  

-------------------------------------------------
architecture behavioral of UART_RX is
  signal t_read_en : std_logic;
  signal t_clk_cnt_en : std_logic;
  signal t_clk_cnt : std_logic_vector(4 downto 0);
  signal t_bit_cnt : std_logic_vector(3 downto 0);
  signal t_dout_vld : std_logic;

begin

  FSM: entity work.UART_FSM(behavioral)
  port map(
    CLK => CLK,
    RST => RST,
    DIN => DIN,
    
    MSB_READ => t_bit_cnt(3),
    CLK_CNT_EN => t_clk_cnt_en,
    CLK_CNT => t_clk_cnt,
    DATA_READ_EN => t_read_en,
    DOUT_VLD => t_dout_vld
  );

  DOUT_VLD <= t_dout_vld;
  process(CLK) begin
    if rising_edge(CLK) then
      
      if RST = '1' then
        t_clk_cnt <= "00000";
        t_bit_cnt <= "0000";
        
      else
        if t_clk_cnt_en = '1' then
          t_clk_cnt <= t_clk_cnt +1;  
        else
          t_clk_cnt <= "00000";
        end if;
        
        if t_read_en = '1' and t_clk_cnt(4) = '1' then
          DOUT(conv_integer(t_bit_cnt)) <= DIN;
          t_clk_cnt <= "00001";
          t_bit_cnt <= t_bit_cnt +1;  
        end if;
        
        if t_read_en = '0' then
          t_bit_cnt <= "0000";
        end if;
        
      end if;
    end if;
  end process;      
end behavioral;
