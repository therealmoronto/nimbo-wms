#!/usr/bin/env bash
set -e

read -p "Enter migration name: " MIGRATION_NAME

if [ -z "$MIGRATION_NAME" ]; then
  echo "Migration name cannot be empty."
  exit 1
fi

echo
echo "Adding migration '$MIGRATION_NAME'..."
echo

dotnet ef migrations add "$MIGRATION_NAME" \
  -p Nimbo.Wms.Infrastructure \
  -c NimboWmsDbContext

echo
read -p "Do you want to update database with new migration? [Yes/No]: " UPDATE_DATABASE_DECISION

case "$UPDATE_DATABASE_DECISION" in
  [Yy]|[Yy][Ee][Ss])
    echo
    echo "Updating database..."
    echo

    dotnet ef database update \
      -p Nimbo.Wms.Infrastructure \
      -c NimboWmsDbContext
    ;;
  *)
    echo
    echo "Database update skipped."
    echo "You can apply it later using:"
    echo "    dotnet ef database update -p Nimbo.Wms.Infrastructure -c NimboWmsDbContext"
    ;;
esac

echo
echo "Done."
