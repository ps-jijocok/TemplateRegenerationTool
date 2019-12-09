#!/bin/sh

configId=$1
handlebarFile=widget_$configId.handlebars
outputFile=widget.js

echo "handlebars $handlebarFile --min --output $outputFile"
handlebars $handlebarFile --min --output $outputFile

echo "perl -pi -e 's/Handlebars./PriceSpider.Handlebars./g' widget.js"
perl -pi -e 's/Handlebars./PriceSpider.Handlebars./g' widget.js

#Remove the handlebarFile after it's been precompiled
echo "rm $handlebarFile"
rm -f $handlebarFile
