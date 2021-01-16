\ Программа для светодиодной маски: 
\ в зависимости от громкости речи меняется ширина светодиодной улыбки.
\
\ Написана для Форт-системы FlashForth.
\ Выполняется на микроконтроллере ATmega328P (плата Arduino Lilypad).
\
\ requires hal/gpio.fs
\ requires hal/adc.fs

-main
marker -main

decimal
5 constant num-pins  \ Кол-во пинов, к которым подключены светодиоды.
2 constant fb-length  \ Длина фильтровочного буфера.

PORTB 5 defpin: PB5
PORTB 2 defpin: PB2
PORTB 1 defpin: PB1
PORTB 0 defpin: PB0
PORTD 7 defpin: PD7

create led-pins ' PB5 , ' PB2 , ' PB1 , ' PB0 , ' PD7 ,
create thresholds 20 , 100 , 240 , 360 , 400 ,  \ Громкостные пороги

create filter-buff fb-length cells allot
variable idx

\ Фильтр "скользящее среднее"
: filter ( volume -- filtered )
    filter-buff  idx @ cells +  !
    idx @ 1+ 
    dup fb-length = if
        drop 0 then
    idx !
    0 
    fb-length for
        filter-buff r@ cells + @
        +
        next
    fb-length / ;

\ Зажечь улыбку в соответствии с громкостью.
: smile ( volume -- )
    num-pins for
        dup
        num-pins r@ - 1-  \ get classic loop index 0 to num-pins - 1
        dup >r
        cells thresholds + @  \ volume, threshold
        >  \ flag
        r> cells led-pins + @ execute  \ flag, mask, port
        rot  \ mask, port, flag
        if high
        else low then
    next 
    drop ;


