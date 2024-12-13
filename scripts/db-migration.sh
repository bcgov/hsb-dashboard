#!/bin/bash
MNAME=$1;

FILE1=./Migrations/$(basename ./Migrations/*_$MNAME.cs);
echo "Updating migration '$FILE1'";

# Check for Mac OS first, and use gsed if so. The built-in getopt is not
# the same as the GNU version, and it does not support long options.
if [ "$(uname)" == "Darwin" ]; then
    echo 'Note: Mac OS X detected. Using gsed. If you do not have this' \
      'installed, run `brew install gsed`.'
    SED_CMD=gsed
else
    SED_CMD=sed
fi

${SED_CMD} -i "2iusing HSB.DAL;" $FILE1;

search=":\ Migration";
replace=":\ PostgresSeedMigration";
${SED_CMD} -i "s/$search/$replace/" $FILE1;

fl1=$(grep -n "protected override void Up(MigrationBuilder migrationBuilder)" $FILE1 | head -n 1 | cut -d: -f1);
l1=$(($fl1 + 2));
${SED_CMD} -i "${l1}i\ \ \ \ \ \ \ \ \ \ \ \ PreUp(migrationBuilder);" $FILE1;

fl=$(grep -n "protected override void Down(MigrationBuilder migrationBuilder)" $FILE1 | head -n 1 | cut -d: -f1);
l2=$(($fl - 3));
${SED_CMD} -i "${l2}i\ \ \ \ \ \ \ \ \ \ \ \ PostUp(migrationBuilder);" $FILE1;

l3=$(($fl + 3));
${SED_CMD} -i "${l3}i\ \ \ \ \ \ \ \ \ \ \ \ PreDown(migrationBuilder);" $FILE1;

eofl=$(wc -l $FILE1 | awk '{ print $1 }');
l4=$(($eofl - 2));
${SED_CMD} -i "${l4}i\ \ \ \ \ \ \ \ \ \ \ \ PostDown(migrationBuilder);" $FILE1;

code -r $FILE1
