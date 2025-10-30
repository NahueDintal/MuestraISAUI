#!/bin/bash

echo "======================================"
echo "Iniciando Empanadas Estelares..."
echo "======================================"

# Verificar si el ejecutable existe
if [ ! -f "bin/Debug/MuestraISAUI.exe" ]; then
    echo "❌ Ejecutable no encontrado. Ejecuta ./build.sh primero."
    exit 1
fi

# Verificar configuración y estado de la base de datos
if [ -f "config.txt" ]; then
    REPOSITORY_TYPE=$(grep -E "^RepositoryType=" config.txt | cut -d'=' -f2)
    if [ "$REPOSITORY_TYPE" = "SQL" ]; then
        echo "📊 Verificando estado de la base de datos..."
        
        # Verificar si el contenedor está ejecutándose
        if ! docker ps | grep -q "sqlserver-dev"; then
            echo "❌ SQL Server no está ejecutándose"
            echo "💡 Iniciando contenedor..."
            docker-compose up -d
            sleep 10
        fi
        
        # Esperar a que SQL Server esté saludable
        echo "⏳ Esperando a que SQL Server esté listo..."
        for i in {1..30}; do
            if docker ps --format "table {{.Names}}\t{{.Status}}" | grep -q "sqlserver-dev.*healthy"; then
                echo "✅ SQL Server conectado y listo"
                break
            elif [ $i -eq 30 ]; then
                echo "⚠️  SQL Server aún no está saludable, pero continuando..."
            fi
            sleep 2
        done
        
        # Verificar conexión con la base de datos
        echo "🔗 Probando conexión a la base de datos..."
        if docker exec sqlserver-dev /opt/mssql-tools/bin/sqlcmd -U sa -P "YourStrong!Password123" -Q "SELECT 1" > /dev/null 2>&1; then
            echo "✅ Conexión a base de datos exitosa"
        else
            echo "❌ No se pudo conectar a la base de datos"
            echo "💡 Verifica que la contraseña en App.config sea correcta"
        fi
    else
        echo "📝 Modo: Memoria (datos temporales)"
    fi
else
    echo "📝 Modo: Memoria (config.txt no encontrado)"
fi

echo ""
echo "🎯 Iniciando aplicación..."
echo "======================================"

mono bin/Debug/MuestraISAUI.exe
