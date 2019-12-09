set configId=%1

echo handlebars "widget_%configId%.handlebars" --min --output "widget.js" 
call handlebars "widget_%configId%.handlebars" --min --output "widget.js"

echo fart "widget.js" "Handlebars." "PriceSpider.Handlebars."
call fart "widget.js" "Handlebars." "PriceSpider.Handlebars."

echo del "widget_%configId%.handlebars"
del "widget_%configId%.handlebars"