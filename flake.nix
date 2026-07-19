{
  description = "Termule dev environment";

  inputs = {
    flake-utils.url = "github:numtide/flake-utils";
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
  };

  outputs =
    {
      self,
      flake-utils,
      nixpkgs,
    }:
    flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = nixpkgs.legacyPackages.${system};

        # These are needed for SharpHook's libUIOHook dependency
        runtimeDeps = [
          pkgs.libX11
          pkgs.libXtst
          pkgs.libXt
          pkgs.libXinerama
        ];
      in
      {
        devShells.default = pkgs.mkShell {
          packages = [
            pkgs.dotnet-sdk_10
          ];

          # Make sure libUIOHook can find its dependencies
          shellHook = ''
            ALL_PATHS="${pkgs.lib.makeLibraryPath runtimeDeps}"
            export LD_LIBRARY_PATH="$ALL_PATHS:$LD_LIBRARY_PATH"
            export NIX_LD_LIBRARY_PATH="$ALL_PATHS:$NIX_LD_LIBRARY_PATH"
          '';
        };
      }
    );
}
