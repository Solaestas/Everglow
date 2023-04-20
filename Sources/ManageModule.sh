#!/bin/bash
echo "Current Module:"
i=0
while read -r line; do
    ((i++))
    reg="<Modules>(.*)</Modules>"
    if [[ "$line" =~ $reg ]]; then
        modules=${BASH_REMATCH[1]}
        index=$i
        echo "${modules//';'/' '}"
    fi
done <Everglow/Module.Build.props
echo "Avaliable Module:"
ms=$(ls Modules/ -I props)
echo "Operations: 
add [name]
remove [name] 
new [name]
exit"
while true; do
    echo -n '>'
    read -r cmd
    if [[ $cmd =~ add\ (.*) ]]; then
        modules="$modules;${BASH_REMATCH[1]}"
        modules=${modules//';;'/';'}
        modules=${modules#';'}
    elif [[ $cmd =~ remove\ (.*) ]]; then
        modules=${modules//${BASH_REMATCH[1]}/''}
        modules=${modules//';;'/';'}
    elif [[ $cmd =~ new\ (.*) ]]; then
        mkdir -p "Modules/${BASH_REMATCH[1]}"
        echo "
    <Project Sdk=\"Microsoft.NET.Sdk\">
      <Import Project=\"..\Module.props\" />
    </Project>
    " >"Modules/${BASH_REMATCH[1]}/Everglow.${BASH_REMATCH[1]}.csproj"
    elif [[ $cmd =~ exit ]]; then
        exit
    else
        echo "error syntex"
        exit
    fi
    echo "Current Module:"
    echo "${modules//';'/' '}"
    sed -i "${index}c <Modules>$modules</Modules>" Everglow/Module.Build.props
done
