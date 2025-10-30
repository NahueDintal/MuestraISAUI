#!/bin/bash
echo "Compilando Casa de Comida Casera..."
echo "======================================"

if grep -q "ApplicationConfiguration.Initialize()" Program.cs; then
    echo "Corrigiendo Program.cs para .NET Framework..."
    sed -i 's/ApplicationConfiguration.Initialize();/Application.EnableVisualStyles();\n            Application.SetCompatibleTextRenderingDefault(false);/g' Program.cs
    echo "Program.cs corregido"
fi

echo "Verificando configuración..."
# Verificar si Docker está ejecutándose
if ! docker ps > /dev/null 2>&1; then
    echo "Docker no está ejecutándose"
    echo "Ejecuta: sudo systemctl start docker"
    exit 1
fi
check_sql_connection() {
    # Intentar ejecutar un comando simple en SQL Server
    docker exec sqlserver-dev /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1
    return $?
}
# Verificar si el contenedor de SQL Server está saludable
if docker ps --format "table {{.Names}}\t{{.Status}}" | grep -q "sqlserver-dev.*healthy"; then
    echo "SQL Server está conectado y saludable"
elif docker ps | grep -q "sqlserver-dev"; then
    echo "SQL Server está ejecutándose pero no saludable (puede estar iniciando)"
    echo "Espera unos segundos y vuelve a intentar"
else
    echo "Contenedor de SQL Server no encontrado"
    echo "Ejecuta: docker-compose up -d"
    exit 1
fi

echo ""
echo "Limpiando builds anteriores..."
rm -rf bin/ obj/

echo "Compilando proyecto..."
msbuild MuestraISAUI.csproj /p:Configuration=Debug /verbosity:minimal

if [ $? -eq 0 ]; then
    echo ""
    echo "¡COMPILACIÓN EXITOSA!"
    if [ "$REPOSITORY_TYPE" = "SQL" ]; then
        echo "Conectado a Base de Datos SQL Server"
    else
        echo "Usando almacenamiento en Memoria"
    fi
    echo ""
    echo "Para ejecutar: ./run.sh"
    echo ""
else
    echo "Error en la compilación"
    exit 1
fi
