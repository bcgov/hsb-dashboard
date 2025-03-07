#!/bin/bash
# More safety, by turning some bugs into errors.
# Without `errexit` you don’t need ! and can replace
# PIPESTATUS with a simple $?, but I don’t do that.
set -o errexit -o pipefail -o noclobber -o nounset

# Check for macOS first, and use gnu-getopt if so. The built-in getopt is not
# the same as the GNU version, and it does not support long options.
if [ "$(uname)" == "Darwin" ]; then
    echo 'Note: macOS detected. Using gnu-getopt. If you do not have this' \
      'installed, run `brew install gnu-getopt`.'
    export PATH="$(brew --prefix gnu-getopt)/bin:$PATH"
fi

# -allow a command to fail with !’s side effect on errexit
# -use return value from ${PIPESTATUS[0]}, because ! hosed $?
! getopt --test > /dev/null
if [[ ${PIPESTATUS[0]} -ne 4 ]]; then
    echo 'I’m sorry, `getopt --test` failed in this environment.'
    exit 1
fi

OPTIONS=s:p:
LONGOPTS=service:,profile:

# -regarding ! and PIPESTATUS see above
# -temporarily store output to be able to check for errors
# -activate quoting/enhanced mode (e.g. by writing out “--options”)
# -pass arguments only via   -- "$@"   to separate them correctly
! PARSED=$(getopt --options=$OPTIONS --longoptions=$LONGOPTS --name "$0" -- "$@")
if [[ ${PIPESTATUS[0]} -ne 0 ]]; then
    # e.g. return value is 1
    #  then getopt has complained about wrong arguments to stdout
    exit 2
fi
# read getopt’s output this way to handle the quoting right:
eval set -- "$PARSED"

s= p=
# now enjoy the options in order and nicely split until we see --
while true; do
    case "$1" in
        -s|--service)
            s="$2"
            shift 2
            ;;
        -p|--profile)
            p="$2"
            shift 2
            ;;
        --)
            shift
            break
            ;;
        *)
            echo "Programming error"
            exit 3
            ;;
    esac
done

# handle non-option arguments
if [[ $# -ne 1 ]]; then
  # If the parameters for 'service' and 'profile' are empty, then default to second argument.
  if [ -z "$s" ] && [ -z "$p" ] && [ ! -z "${2-}" ]; then
    s=$2
  fi
  # if [ ! -z "$s" ]; then echo "service=$s"; fi
  # Invalid arguments provided
  # echo "$0: A single input file is required."
  # exit 4
fi

# echo "arguments: service: $s, profile: $p"
