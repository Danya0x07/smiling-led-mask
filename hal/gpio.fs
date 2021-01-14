\ Слой абстракции для работы с портами ввода-вывода.

-hal-gpio
marker -hal-gpio

\ Для наших целей достаточно этих констант.
$25 constant PORTB
$2b constant PORTD

\ Определяющее слово для объявления пинов в удобном формате.
\ Например, чтобы объявить пин 7 порта D:
\   PORTD 7 defpin PD7
: defpin: ( PORTx #pin --- <word> | <word> --- mask port)
    1 swap lshift  \ Преобразовать номер пина в битовую маску.
    create
        c, c,  \ Запоминаем адрес порта и битовую маску пина.
    does>
        dup c@      \ Достаём битовую маску пина.
        swap 1+ c@  \ Достаём порт.
;

\ Установка пина в режим выхода.
: mode-output ( pinmask portaddr -- )
    \ Воспользуемся тем, что для любого x соблюдается равенство адресов
    \ PORTx == DDRx + 1 == PINx + 2
    1-
    mset
;

\ Установить высокий лог. уровень на пине.
: high ( pinmask portaddr -- )
    mset
; inlined

\ Установить низкий лог. уровень на пине.
: low ( pinmask portaddr -- )
    mclr
; inlined
