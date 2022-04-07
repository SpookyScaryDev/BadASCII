using System;
using System.Linq;
using System.Collections.Generic;
using BadASCII;

namespace TextAdventure {

class Renderer {
    enum BufferType {
        UserInterface = 0
    }

    private static TextBuffer[] mBuffers;

    private static TextBuffer mMenuImage;

    public static void Init() {
        int width = 150;
        int height = 50;

        mBuffers = new FastTextBuffer[Enum.GetNames(typeof(BufferType)).Length];
        for (int i = 0; i < mBuffers.Length; i++) {
            mBuffers[i] = new FastTextBuffer(width, height);
        }

                    string house =
                @"
                                                                           d-                                                                         
                                                                          :MM/                                                                        
                                                                          sMMN/                                                                       
                                                                          mMMMN-                                                                      
                                                                         `NMMMMN.                                                                     
                                                                         -MMMMMMd`                                                                    
                                                                         +MMMMMMMh`                                                                   
                                                                         dMMMMMMMMy`                                                                  
                                                                        :MMMMMMMMMMh.                                                                 
                                                                       `dMMMMMMMMMMMm:                                                                
                                                                      `hMMMMNhhhhhhMMMs.                                                              
                                                                     `hMMMMMh++++++hMMMNs-                                                            
                                                                    :mMMMMMMy+++++++dMMMMMh/`                                                         
                                                                  -yMMMMMMMMo+++++++oNMMMMMMNs:                                                       
                                                                  .:+ydNMMMMmmmmmmmmmNMMMy-.`                                                         
                                                                       `mMMMMMMMMMMMMMMMo                                                             
                                                                        .mMMMMMMMMMMMMMN`       /`                                                    
                                                            o:`          -NMMMMMMMMMMMMm       +Mh-                                                   
                                                           `MNms:`        +MMMMMMMMMMMMN`    `sMMMNh/.                                                
                                                          `sMMMMMmy/-`    .MMMMMMMMMMMMM:  .+mMMMMMMMNds+:-`                                          
                            .                       ``.-/ymMMMMMMMMMNmho/:sMMMMMMMMMMMMMs/smMMMMMMNdhss+//.`                                          
                           :Ns.                     -:+shdmNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM/`                                                  
                          `mMMm/`                         `.:+hmMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMd                             .:                     
                          yMMMMMy-                             `oMMMMMMMMMmmmmmMMMMMMMNmddMMMMMMMs                          ./ymMs                    
                         :MMMMMMMNy.                             sMMMMMMMd+++++NMMMMhso++sMMMMMMM:                       ./yNMMMMM+                   
                         dMMMMMMMMMm+`                           `NMMMMMMy++++oMMMMMo++++oMMMMMMN`                    `:yNMMMMMMMMm`                  
                        /MMMMMMMMMMMMmo.                          sMMMMMMo++++sMMMMMs+++++NMMMMMh                  `/yNMMMMMMMMMMMM/                  
                        mMMMMMMMMMMMMMMNs`                        /MMMMMN+++++hMMMMMs+++++NMMMMMs               `:yNMMMMMMMMMMMMMMMh                  
                       +MMMMMMMMMMMMMMMMMMy:                      -MMMMMd+++++dMMMMMy+++++mMMMMM/             :yNMMMMMMMMMMMMMMMMMMm`                 
                      `NMMMMMMMMMMMMMMMMMMMMd+`                   -MMMMMy+++++NMMMMMy+++++dMMMMM/         `/yNMMMMMMMMMMMMMMMMMMMMMM.                 
                      oMMMMMMMMMMMMMMMMMMMMMMMNy:                 oMMMMMo++++oMMMMMMy+++++hMMMMMy      `/yNMMMMMMMMMMMMMMMMMMMMMMMMM+                 
                     .MMMMMMMMMMMMMMMMMMMMMMMMMMMms-              yMMMMMdddddmMMMMMMNNNNNNMMMMMMN`  `/yNMMMMMMMMMMmyosymMMMMMMMMMMMMh                 
                     dMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNms:`         `NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMs:yNMMMMMMMMMMMMdo++++ohMMMMMMMMMMMN`                
                    oMMMMMMMMMMMMMMmyssydNMMMMMMMMMMMMMmy/.      :MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNo+++++++dMMMMMMMMMMM+                
                   /MMMMMMMMMMMMMds++++++oyNMMMMMMMMMMMMMMNdo:.  yMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMh++++++++mMMMMMMMMMMMN.               
                  :NMMMMMMMMMMMNy++++++++++odMMMMMMMMMMMMMMMMMmdsNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNo++++++++MMMMMMMMMMMMMd.              
                 /NMMMMMMMMMMMMd++++++++++++sMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMd++++++++sMMMMMMMMMMMMMMm:             
               `+MMMMMMMMMMMMMMNo++++++++++++NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMo++++++++yMMMMMMMMMMMMMMMNh-           
              -hMMMMMMMMMMMMMMMMd++++++++++++dMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMddddhhhyymMMMMMMMMMMMMMMMMMNh/.`       
            `oNMMMMMMMMMMMMMMMMMMo+++++++++++sMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNhs/.``  
          `/dMMMMMMMMMMMMMMMMMMMMd++++++++ooooMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNNNddhyyyo+:.
        `+mMMMMMMMMMMMMMMMMMMMMMMMhdddmmmmNNNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMmho/:.`           
      -sNNmhhssssssyhhNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMdhhhhhhhhhhhhhmMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNs/.                  
    -//-`             `+NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMo+++++++++++++hMMMMMMMMmddddddddddddddddMMMMMMMMMMMMMMMd-                      
                        `sMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMN++++++++++++++hMMMMMMMMy+++++++++++++++oMMMMMMMMMMMMMMM:                       
              `           :NMMMMMMMMMMMMNmmmmmmmmmmmmmmmmmMMMMMMMMMMMMm++++++++++++++hMMMMMMMMh+++++++++++++++sMMMMMMMMMMMMMMM`                       
            `yNy/`         .dMMMMMMMMMMMh++++++++++++++++oMMMMMMMMMMMMd++++++++++++++hMMMMMMMMh+++++++++++++++hMMMMMMMMMMMMMMh                        
           :dMMMMmy/.       `sMMMMMMMMMMd++++++++++++++++sMMMMMMMMMMMMy++++++++++++++hMMMMMMMMd+++++++++++++++dMMMMMMMMMMMMMM+                        
        `/hNMMMMMMMMNho:-.````oMMMMMMMMMm++++++++++++++++yMMMMMMMMMMMMy++++++++++++++hMMMMMMMMd+++++++++++++++mMMMMMMMMMMMMMM.                        
     ./ymMMMMMMMMMMMMMMMNmmmmmdMMMMMMMMMM++++++++++++++++dMMMMMMMMMMMMo++++++++++++++hMMMMMMMMm+++++++++++++++NMMMMMMMMMMMMMm`                        
`./shmmNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMs+++++++++++++++NMMMMMMMMMMMM+++++++++++++++hMMMMMMMMm++++++++++++++oMMMMMMMMMMMMMMd                         
`.----..---:+yMMMMMMMMMMMMMMMMMMMMMMMMMMMy++++++++++++++oMMMMMMMMMMMMNsssssssssssssssdMMMMMMMMN++++++++++++++sMMMMMMMMMMMMMMs                         
              sMMMMMMMMMMMMMMMMMMMMMMMMMMd++++++++++++++sMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMyyyyyyyyyyyyyydMMMMMMMMMMMMMMo                         
               sMMMMMMMMMMMMMMMMMMMMMMMMMm++++++++++++++yMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMo                         
                yMMMMMMMMMMMMMMMMMMMMMMMMNo+++++++++++++dMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMh                         
                `hMMMMMMMMMMMMMMMMMMMMMMMMs+++++++++++++NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMm                         
                 `dMMMMMMMMMMMMMMMMMMMMMMMy++++++++++++oMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM-                        
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
                  `mMMMMMMMMMMMMMMMMMMMMMMd++++++++++++sMMMMNmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmhhhhhhhhh+                                                                                                                                                                                                                                                                                                                                                                                                                                          
        ";

            string trees =
    @"                                                                                                                                                                                                                                                                                                                                                                                                                      
                                            .                                                                                                                                                                                                          
                                    `.       /: -.                                                                                                                                                                                                       
                        `.`-. ``../-- .   ooo:--``-                                                                                                                             ` -                                                                   
                        `+-o.`/+. oy .+   /N-  s/s/`.                   `  `o/`                                                                                                 +o.           `                                                       
                            y+ -s/  +h/y/+` sm  -ds.`.`         +  - +-/ //- :d/                                                                                             ./..`+h   -:      `h-     ``` `                                            
                            .yy/d   .Ny` `` +N:`dN-             .:++ sy` +o `hs`:                                                                                          `/sy+ommo `:-   `/. :d:.    `.//s                                            
                            /dd   yd.   .+dNmmmo -: /`  - .`    -d/os /m..yms:`                                                                                                :ddo/.   `s-++y.        `dy `.                                         
                                .N/-sN.  /hNNh/-.` -d .y:` /+y`/.   :hNh:Nmss+.                                                                                  /:`               `my         N:          -Ny+`                                         
                                omNNd .dNNd:`..`   msh.    yy h`o.   yNNh-           `                                                                       `--:://+:`         .odNNy-      `Nds/h+       ms  `                                        
                                -smNdhNNN:  oy.-  +N:  `  /m/dy/-. .hm+ +.`  - . +s o.o                                                                     -`    /sohh+:.``.-ommyoohmmdh`  :Ny `-s      `N+  .o                                       
                            :.` ` -mNNNNy   `mos- -N/ .+:. omN.   omy.  d+.. ++-+-s:ds+-  `                                                                   `-:ys  `.smhysso+:`    ./mNh/omy`          oNds+/+                                       
                            :s` o`.-hNNN:   `my`  `hy .ho...mNdo/yNo   /N-h//`y+o-ms-  o:s-                                                                    .:.+  `sh+``            +NNNm/          .omm/`                                          
                            so-dy:`-NNN+ ./hNmy/:-omdyo.`` .+ymNNm` `+mdo+/:``ohhy    dh---`                                                                     .  -m.                sNNh      ./yhdhy+.                                            
                            `.+ds. `mNNmhmNmdhhdmmmd/.`.`+ ` `.hNd.odh/`      `mm.   oh.`yo.                                                                        +y    ``   `       `sNms++oshmNNd:`       .::-..                                  
                                -Ns  yNNNNN+.```..--` .+s:/:-   +Nmmm+`       .sm:.-/yNyhdh-.-.                                                                      hs+   /hhhyh-       :NNNNNmddmNNd.     .:smh-`                                    
                                `mm. +NNNNh   . +:  `.``/d /o  `.mNNs     .:+ydNmddhyoo+/+yds/:.                                                                    +-`h.  +NNNNm`       /NNNNm/`.-/ymdysosyyo:/hh-                                    
                                :syyyNNNNy   o.s:/o/-` :m-d.`:: oNNo    .dNNds+///.`     .hhh:-                                                                    .```` .mNNNNN:       hNNNm/      .--:/:-.  `sdd.                                   
                                    `+mNNNNN.  hm/os+o-  :Ndmssy.  dNmo-`:dmy/.  /o-o.`      ./.                                                                           -NNNNNN/      :NNNNs`//   :.     `//+yy`os                                   
                                    :sdmNNd:+Ny/-  -  .hNd/` .o` .ymNNmNm/     dy.h/y/.                                                                                ``-mNNNNm:....:odNNNNsyyyo/+h      `:o-::.oh                                   
                                .os-      -yNNNNm`     .odm+`         -sNNN+..    ddhy:.`.         `                                                                     -hmmNNNNNNNNNNNNNNNNNNd.  `  s+`    .` `. ` o                                   
                                :hhy/-     yNNNd-`  `yNNN/            `mNm +so-` -NN.:-.s/ +    ./:                                                                     .://+mNNNddmmmNNNNNNNNo     -+-.            :                                   
                                ::+oy     .mNNNNNmsyNNNm            `sNNm-d:-.``-NN-.yyo-`y:.  .y`//+ /`                                                                   :hmNmh``.:NNNNNNNN-         .                                               
                                            `/sdNNNNNNNNm`        .-+dNNNNmd+oy-``mNhms`  :s- -.m: :Nshy``                                                             ` //   .`.     hNNNNNNN`     //./-                                               
                                :ydo.--`/`     - -yNNNNNNN+   .:+ymmNNNNNNm/  `-d-`hNN+    /+o+.+Nmds/-..-`                                                             :/d-           /NNNNNNN-     :No`/syys:                                          
                            `-oymdmhyNd- ./:o/. sNNNNNNy +dmmNNNNNNNNds-     :` yNh     hm- /mm+`                                                                     `yd           `mNNNNNNo      sddo```/ds.``--                                    
                                `-yso/+:/  `sm+  `hNNNNNNsmNNNNNNhs/-.`         .dNo     mNshmm+                                                                      --oN:           yNNNNNNd      -Nm     -d+ohy`                                    
                                    -         `No   /NNNNNNNNNNNNNs:`           .odNNh:-:+yNNNh+.                                                                         `od:         `mNNNNNNm-     :Nm      :  +s                                     
                                            dm:  -NNNNNNNNNNNNNNmd+`        -dNNNNNmmNdhhs:` `                                                                        `  omo-    `.-oNNNNNNNNm+---:hNN`        .                                      
                                            -hms.oNNNNNNNNNNNNNNNNNy+/.    -dNNNds/::-.``   .+`                                                                       o:syhdmhssyhdmNNNNNNNNNNNmmmmdy/                                                
                                                `+NmmNNNNNNNNNNNNNNNNNNNNdsoshmNNms.         `-h:-.                                                                      `:-``.:+syhhmNNNNNNNNNNNNNNs-`                                                  
                                                dNNNNNNNNNds/::-:smNNNNNNNNNNNNo        :syhd+.-.                                                                                 `.:smNNNNNNNNNNN-                                                    
                                                hNNNNNNNm+``      -ydmmmmmmmNNN+`     .oms:-/++s.                                                                                     .hNNNNNNNNNNh                                                    
                                                dNNNNNNm- :s-:   ````-------/hmNho::+ymNy-`  +:                                                                                        `dNNNNNNNNNm                                                    
                                                mNNNNNN+ :N/-`-``oh+/.-       :hmNNmmy++sshsoh---                                                                                       +NNNNNNNNNd                                                    
                                                -NNNNNNN- smssh+-.`-hyyy.   ``   +Nm:.     `-/yN/..`                                                                                     -NNNNNNNNNy                                                    
                                            `omNNNNNNNooms` `o+   `  ```:.mmh+ `No           omd:`                                                                                     :NNNNNNNNN:                                                    
                                            hNNNNNNNNNNNs     `     +m/ymmNs.`  hs-.-`         .-`                                                                                     hNNNNNNNNy                                                     
                                            `NNNNNNNNNNm/           :hhyhom.`   `h`-+                                                                                                  /NNNNNNNNN.                                                     
                                            -NNNNNNNNNy.            ``    .`    :`. `                                                                                                 :mNNNNNNNNm                                                      
                                            +NNNNNNNNm`                                                                                                                              .mNNNNNNNNNm                                                      
                                            hNNNNNNNNN`                                                                                                                              yNNNNNNNNNNN                                                      
                                            /NNNNNNNNNNo                                                                                                                             `mNNNNNNNNNNN.                                                     
                                        .-` -mNNNNNNNNNNmo-/.                                                                                                                         +NNNNNNNNNNNNs.                                                    
                                    .dmhymNNNNNNNNNNNNNmNm:                                                                                                                      `/mNNNNNNNNNNNNNdo                                                   
                                    yNNNNNNNNNNNNNNNNNNNNNm.                                                                                                                   `/hNNNNNNNNNNNNNNNNN/                                                  
                                    ``/NNNNNNNNNNNNNNNNNNNNNNNdy/                                                                                                                -hNNNNNNNNNNNNNNNNNNNms:`                                               
                                `ydmNNNNNNNNNNNNNNNNNNNNNNNNNNo+o-`                                                                                                       `./ymNNNNNNNNNNNNNNNNNNNNNNNy-``                                            
                                `-sNmNmmmyoyNNNNNNNdNNNNmNmmNNNmNmmmds-`                                                                                                `:+shmNNNNNNNmmmNNNNNNmmNNNNNNNNNmdhy+-`                                        
                                +//s:/:::ydmmNhymmo.shy+-o/`+yo:/--/:oo///                                                                                             ohs+/+odssyys+-`/dmhdh+::+hmmdssyyo++oyhy+`                                      
                                        -:+o.:/  ``                                                                                                                    `:                ``        -:                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        ";

        string title = @"
 ██░ ██  ▒█████   █    ██   ██████ ▓█████     ▒█████    █████▒    ██░ ██  ▒█████   ██▀███   ██▀███   ▒█████   ██▀███    ██████ 
▓██░ ██▒▒██▒  ██▒ ██  ▓██▒▒██    ▒ ▓█   ▀    ▒██▒  ██▒▓██   ▒    ▓██░ ██▒▒██▒  ██▒▓██ ▒ ██▒▓██ ▒ ██▒▒██▒  ██▒▓██ ▒ ██▒▒██    ▒ 
▒██▀▀██░▒██░  ██▒▓██  ▒██░░ ▓██▄   ▒███      ▒██░  ██▒▒████ ░    ▒██▀▀██░▒██░  ██▒▓██ ░▄█ ▒▓██ ░▄█ ▒▒██░  ██▒▓██ ░▄█ ▒░ ▓██▄   
░▓█ ░██ ▒██   ██░▓▓█  ░██░  ▒   ██▒▒▓█  ▄    ▒██   ██░░▓█▒  ░    ░▓█ ░██ ▒██   ██░▒██▀▀█▄  ▒██▀▀█▄  ▒██   ██░▒██▀▀█▄    ▒   ██▒
░▓█▒░██▓░ ████▓▒░▒▒█████▓ ▒██████▒▒░▒████▒   ░ ████▓▒░░▒█░       ░▓█▒░██▓░ ████▓▒░░██▓ ▒██▒░██▓ ▒██▒░ ████▓▒░░██▓ ▒██▒▒██████▒▒
 ▒ ░░▒░▒░ ▒░▒░▒░ ░▒▓▒ ▒ ▒ ▒ ▒▓▒ ▒ ░░░ ▒░ ░   ░ ▒░▒░▒░  ▒ ░        ▒ ░░▒░▒░ ▒░▒░▒░ ░ ▒▓ ░▒▓░░ ▒▓ ░▒▓░░ ▒░▒░▒░ ░ ▒▓ ░▒▓░▒ ▒▓▒ ▒ ░
 ▒ ░▒░ ░  ░ ▒ ▒░ ░░▒░ ░ ░ ░ ░▒  ░ ░ ░ ░  ░     ░ ▒ ▒░  ░          ▒ ░▒░ ░  ░ ▒ ▒░   ░▒ ░ ▒░  ░▒ ░ ▒░  ░ ▒ ▒░   ░▒ ░ ▒░░ ░▒  ░ ░
 ░  ░░ ░░ ░ ░ ▒   ░░░ ░ ░ ░  ░  ░     ░      ░ ░ ░ ▒   ░ ░        ░  ░░ ░░ ░ ░ ▒    ░░   ░   ░░   ░ ░ ░ ░ ▒    ░░   ░ ░  ░  ░  
 ░  ░  ░    ░ ░     ░           ░     ░  ░       ░ ░              ░  ░  ░    ░ ░     ░        ░         ░ ░     ░           ░                                                       
";


        mMenuImage = new FastTextBuffer(250, 100);

        mMenuImage.Blit(house, new Vector2i(0, -15), ConsoleColor.Yellow);
        mMenuImage.Blit(trees, new Vector2i(-45, 0), ConsoleColor.DarkMagenta, true);
        mMenuImage.Blit(title, new Vector2i(12, 0), ConsoleColor.DarkRed, true); 
    }

    static public void DrawBox(Vector2i position, int width, int height) {
        TextBuffer box = new FastTextBuffer(width, height);

        string boxMessage = "╔" + string.Concat(Enumerable.Repeat("═", box.width-2)) + "╗" + "\n";
        for (int i = 0; i  < height-2; i++) {
            boxMessage += "║" + string.Concat(Enumerable.Repeat(" ", box.width-2)) + "║" + "\n";
        }
        boxMessage += "╚" + string.Concat(Enumerable.Repeat("═", box.width-2)) + "╝";


        box.Blit(boxMessage, new Vector2i(0, 0));

        mBuffers[(int)BufferType.UserInterface].Blit(box, position);
    }

    static public void DrawTextBox(string message, Vector2i position) {
        List<string> splitMessage = message.Split('\n').ToList<string>();

        for (int i = 0; i < splitMessage.Count(); i++) {
            if (splitMessage[i] == "") {
                splitMessage.RemoveAt(i);
                continue;
            }
            // if (!string.IsNullOrWhiteSpace(splitMessage[i])) splitMessage[i] = splitMessage[i].TrimStart();
        }

        string boxMessage = "";
        for (int i = 0; i  < splitMessage.Count(); i++) {
            boxMessage += splitMessage[i] + "\n";
        }
        //boxMessage = boxMessage.Remove(boxMessage.Length - 1);

        List<string> splitMessagesplitMessageFormatRemoved = new List<string>(splitMessage);

        bool doneRemovingFormat = false;
        while (!doneRemovingFormat) {
            doneRemovingFormat = true;
            for (int i = 0; i < splitMessagesplitMessageFormatRemoved.Count; i++) {
                if (splitMessagesplitMessageFormatRemoved[i].Contains("{")) {
                    doneRemovingFormat = false;
                    splitMessagesplitMessageFormatRemoved[i] = splitMessagesplitMessageFormatRemoved[i].Remove(splitMessagesplitMessageFormatRemoved[i].IndexOf("{"), 4);
                }
                if (splitMessagesplitMessageFormatRemoved[i].Contains("}")) {
                    doneRemovingFormat = false;
                    splitMessagesplitMessageFormatRemoved[i] = splitMessagesplitMessageFormatRemoved[i].Remove(splitMessagesplitMessageFormatRemoved[i].IndexOf("}"), 1);
                }
            }
        }

        TextBuffer box = new FastTextBuffer(splitMessagesplitMessageFormatRemoved.Max(s => s.Length), splitMessagesplitMessageFormatRemoved.Count());


        //box.Blit(boxMessage, new Vector2i(0, 0));
        Vector2i charPos = new Vector2i(0, 0);
        ConsoleColor colour = ConsoleColor.Gray;
        for (int i = 0; i < boxMessage.Length; i++) {
            if (boxMessage[i] == '{') {
                colour = (ConsoleColor)int.Parse(boxMessage.Substring(i+1, 3));
                i += 3;
            }
            else if (boxMessage[i] == '}') {
                colour = ConsoleColor.Gray;
            }
            else if (boxMessage[i] == '\n') {
                charPos.x = 0;
                charPos.y++;
            }
            else {
                box.Blit(boxMessage[i], charPos, colour);
                charPos.x++;
            }
        }

        DrawBox(position, splitMessagesplitMessageFormatRemoved.Max(s => s.Length) + 4, splitMessagesplitMessageFormatRemoved.Count() + 4);

        position.x += 2;
        position.y += 2;

        mBuffers[(int)BufferType.UserInterface].Blit(box, position);
    }

    static public void DrawLocationInfo(string info) {
        string[] splitMessage = info.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        string finalMessage = "";

        for (int i = 0; i < 32; i++) {
            if (i < splitMessage.Length) { 
                if (!string.IsNullOrWhiteSpace(splitMessage[i])) splitMessage[i] = splitMessage[i].TrimStart();
                finalMessage += splitMessage[i].PadRight(40) + '\n';
            }
            else {
                finalMessage += " " + '\n';
            }
        }

        Vector2i position = new Vector2i(0, 7);
        DrawTextBox(finalMessage, position);
    }

    static public void DrawLocationName(string name) {
        int boxWidth = name.Length + 4;
        Vector2i position = new Vector2i(0, 2);
        //position.x = (mBuffers[(int)BufferType.UserInterface].width / 2) - (boxWidth / 2);
        DrawTextBox(name.PadRight(40), position);
    }

    static public void DrawSpecialInfo(string message) {
        List<string> splitMessage = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        string finalMessage = "";

        for (int i = 0; i < splitMessage.Count(); i++) {
            if (splitMessage[i] == "") {
                splitMessage.RemoveAt(i);
                continue;
            }
            if (!string.IsNullOrWhiteSpace(splitMessage[i])) splitMessage[i] = splitMessage[i].TrimStart();
        }

        for (int i = 0; i < splitMessage.Count(); i++) {
            finalMessage += splitMessage[i].PadRight(mBuffers[(int)BufferType.UserInterface].width-4) + '\n';
        }

        int boxHeight = splitMessage.Count() + 4;
        Vector2i position = new Vector2i(0, 0);
        position.y = (mBuffers[(int)BufferType.UserInterface].height - 7 - boxHeight);
        DrawTextBox(finalMessage, position);

        Vector2i joinLinePos = new Vector2i(0, position.y - 1);
        mBuffers[(int)BufferType.UserInterface].Blit("║" + string.Concat(Enumerable.Repeat(" ", 42)) + "╚", position);

        joinLinePos.x = mBuffers[(int)BufferType.UserInterface].width - 20 - 4;
        mBuffers[(int)BufferType.UserInterface].Blit("╚" + string.Concat(Enumerable.Repeat("═", 22)) + "╝", joinLinePos);
    }

    static public void DrawInventory() {
        PlayerData player = Program.GetPlayerInfo();

        string[] splitMessage = new string[player.inverntory.Count()]; 
        string finalMessage = "";

        string line;
        for (int i = 0; i < 33; i++) {
            if (i < player.inverntory.Count()) { 
                var item = player.inverntory.ElementAt(i);
                line = "";
                if (item.Value > 0) {
                    line = "{11 " + item.Value + "x " + item.Key + "}"; 
                    finalMessage += line + '\n';
                }
            }
            else {
                finalMessage += string.Concat(Enumerable.Repeat(" ", 20)) + '\n';
            }
        }


        Vector2i position = new Vector2i();
        position.x = mBuffers[(int)BufferType.UserInterface].width - 20 - 4;
        position.y = 7;
        DrawTextBox(finalMessage, position);
    }

    static public void DrawInputMessage(string message) {
        Vector2i position = new Vector2i(2, mBuffers[(int)BufferType.UserInterface].height-5);
        mBuffers[(int)BufferType.UserInterface].Blit(message, position);
    }

    static public void DrawInputBox() {
        Vector2i position = new Vector2i(0, mBuffers[(int)BufferType.UserInterface].height-7);
        DrawBox(position, mBuffers[(int)BufferType.UserInterface].width, 7);
    }

    static public void DrawCurrentInput() {
        Vector2i position = new Vector2i(2, mBuffers[(int)BufferType.UserInterface].height-3);
        mBuffers[(int)BufferType.UserInterface].Blit(">> " + Program.GetCurrentCommand() + "_", position);
    }

    static public void DrawCompass(bool north, bool south, bool east, bool west) {
        Vector2i compassPosition = new Vector2i(mBuffers[(int)BufferType.UserInterface].width - 24, 0);
        Vector2i position = new Vector2i(0, 0);
        TextBuffer compass = new FastTextBuffer(24, 7);
        compass.Blit("o──────────────────────o", position);
        position.y++;

        compass.Blit("│          N           │", position);
        position.y++;

        compass.Blit("│                      │", position);
        position.y++;

        compass.Blit("│      W   O   E       │", position);
        position.y++;

        compass.Blit("│                      │", position);
        position.y++;

        compass.Blit("│          S           │", position);
        position.y++;

        compass.Blit("o──────────────────────o", position);
        position.y++;

        Vector2i arrowPos = new Vector2i();
        if (north) {
            arrowPos.x = 11;
            arrowPos.y = 2;
            compass.Blit('^', arrowPos); 
        }  
        if (south) {
            arrowPos.x = 11;
            arrowPos.y = 4;
            compass.Blit('v', arrowPos); 
        }  
        if (east) {
            arrowPos.x = 13;
            arrowPos.y = 3;
            compass.Blit('>', arrowPos); 
        }  
        if (west) {
            arrowPos.x = 9;
            arrowPos.y = 3;
            compass.Blit('<', arrowPos); 
        }  

        mBuffers[(int)BufferType.UserInterface].Blit(compass, compassPosition);
    }

    static public void DrawHealth() {
        PlayerData player = Program.GetPlayerInfo();
        int maxHealth = player.maxHealth;
        int health = player.health;

        Vector2i position = new Vector2i(0, 0);
        TextBuffer healthBar = new FastTextBuffer(9 + maxHealth, 1);

        healthBar.Blit("Vitality:", position);
        position.x = "Vitality: ".Length;
        healthBar.Blit(string.Concat(Enumerable.Repeat("█", health)), position, ConsoleColor.DarkRed);
        position.x = "Vitality: ".Length + health;
        healthBar.Blit(string.Concat(Enumerable.Repeat("█", maxHealth - health)), position, ConsoleColor.Gray);

        mBuffers[(int)BufferType.UserInterface].Blit(healthBar, new Vector2i(0, 0));
    }

    static public void DrawStatus() {
        PlayerData player = Program.GetPlayerInfo();
        bool poisoned = player.piosoned;

        string statusMessage = "Status:  ";
        if (poisoned) statusMessage += " Poisoned";

        Vector2i position = new Vector2i(0, 0);
        TextBuffer status = new FastTextBuffer(8 + statusMessage.Length, 1);

        status.Blit("Status:   ", position);
        position.x += "Status:  ".Length;

        if (poisoned) status.Blit(" Poisoned", position, ConsoleColor.Magenta);

        mBuffers[(int)BufferType.UserInterface].Blit(status, new Vector2i(0, 1));
    }

    static public void DrawHelp() {
        string helpScreen =
            "                 {10 Instructions}                   " + '\n' +
            " " + '\n' +
            "This is a {11 text adventure}, in which you must use " + '\n' +
            "        your wits and cunning to survive!            "      + '\n' +
            " "      + '\n' +
            "To play, you must read the {11 location descriptions}" + '\n' +
            " as you explore and respond by typing what you" + '\n' +
            "  want to do. To do this, type a {09 single verb},   " + '\n' +
            "  followed by {14 another word}, then press enter.   " + '\n' +
            " " + '\n' +
            "                 For example:                       " + '\n' +
            "> {09 go} {14 north}                                 " + '\n' +
            "> {09 go} {14 up}                                 " + '\n' +
            "> {09 take} {14 berries}                             " + '\n' +
            "> {09 eat} {14 berries}                              " + '\n' +
            " " + '\n' +
            "      The game accepts the following verbs:          " + '\n' +
            "     {09 go}, {09 take}, {09 use}, {09 eat}, {09 drink}, {09 search}, {09 give}  " + '\n' +
            " " + '\n' +
            "Certain important words are {09 highlighted}, and the " + '\n' +
            "   directions you can go are indicated by the        " + '\n' +
            "                    {14 compass}.                    " + '\n' +
            " " + '\n' +
            "   You can type {09 help} to see this screen again." + '\n' +
            " " + '\n' +
            "        Your adventure awaits. Good Luck!            " + '\n' +
            " " + '\n' +
            "           {10 Press any key to continue...}         ";

        string finalMessage = "";
        string[] splitString = helpScreen.Split('\n');
        for (int i = 0; i < splitString.Length; i++) {
            if (!string.IsNullOrWhiteSpace(splitString[i])) splitString[i] = splitString[i].TrimEnd();
            finalMessage += splitString[i] + '\n';
        }

        Vector2i position = new Vector2i(0, 0);
        position.x = mBuffers[(int)BufferType.UserInterface].width / 2 - 26;
        position.y = mBuffers[(int)BufferType.UserInterface].height / 2 - splitString.Length / 2 - 4;

        mBuffers[(int)BufferType.UserInterface].Clear('░');
        DrawTextBox(finalMessage, position);
    }

    static public void DrawMenu() {
        string lightning = @"
                                  dZZZZZ,                   
                                 dZZZZ  ZZ,
                         ,AZZZZZZZZZZZ  `ZZ,_           
                    ,ZZZZZZV'      ZZZZ   `Z,`\
                  ,ZZZ    ZZ        ZZZZ   `V
               ZZZZV'     ZZ         ZZZZ    \_               
               V   l       ZZ        ZZZZZZ           
               l    \       ZZ,     ZZZ  ZZZZZZ,
              /            ZZ l    ZZZ    ZZZ `Z,
                          ZZ  l   ZZZ     Z Z, `Z,             
                         ZZ      ZZZ      Z  Z, `l
                         Z        ZZ      V  `Z   \
                         V        ZZC     l   V
                         l        V ZR        l       
                          \       l  ZA
                            \         C           
                                      K
                                     /
            ";

        Random rnd = new Random();
        mBuffers[0].Clear();
        if (rnd.Next(1, 7) % 6 == 0) {
            mBuffers[0].Blit(lightning, new Vector2i(0, -5), ConsoleColor.White, true);
            mBuffers[0].Blit(lightning, new Vector2i(80, -5), ConsoleColor.White, true);
        }
        mBuffers[0].Blit(mMenuImage, new Vector2i(0, 0), true);
        mBuffers[0].Blit("                                     ", new Vector2i(61, 10), ConsoleColor.DarkRed);
        mBuffers[0].Blit(" press any key to begin...           ", new Vector2i(61, 11), ConsoleColor.DarkRed);
        mBuffers[0].Blit("                                     ", new Vector2i(61, 12), ConsoleColor.DarkRed);
    }

    static public void DrawEnd() { 
         string endScreen =
            "You find your friend tied up behind the boxes. You" + '\n' +
            "  quickly untie them and you make your escape." + '\n' +
            " " + '\n' +
            "Congratulations! You escaped the house of horrors!" + '\n' + 
            " " + '\n' +
            "           {10 Press any key to continue...}         ";

        string finalMessage = "";
        string[] splitString = endScreen.Split('\n');
        for (int i = 0; i < splitString.Length; i++) {
            if (!string.IsNullOrWhiteSpace(splitString[i])) splitString[i] = splitString[i].TrimEnd();
            finalMessage += splitString[i] + '\n';
        }

        Vector2i position = new Vector2i(0, 0);
        position.x = mBuffers[(int)BufferType.UserInterface].width / 2 - 26;
        position.y = mBuffers[(int)BufferType.UserInterface].height / 2 - splitString.Length / 2 - 4;

        mBuffers[(int)BufferType.UserInterface].Clear('░');
        DrawTextBox(finalMessage, position);
    }

    static public void StartFrame() {
        for (int i = mBuffers.Length-1; i >= 0; i--) {
            mBuffers[i].Clear();
        }

        DrawInputBox();
        DrawInventory();
    }

    public static void EndFrame() {
        if (Program.GetGameState() == GameState.Explore) {
            DrawInputMessage(Program.GetLastMessage());
            DrawCurrentInput();
            DrawHealth();
            DrawStatus();
        }

        for (int i = mBuffers.Length-1; i >= 0; i--) {
            Console.SetCursorPosition(0, 0);
            mBuffers[i].Print();
        }
    }


}

}
