const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = function (outDir) {
  return {
    context: __dirname,
    entry: "./index.js",
    plugins: [
      new HtmlWebpackPlugin({
        publicPath: "/",
        template: "./index.html",
      }),
    ],
    module: {
      rules: [
        {
          test: /\.css$/i,
          use: ["style-loader", "css-loader"],
        },
        {
          test: /\.s[ac]ss$/i,
          use: [
            // Creates `style` nodes from JS strings
            "style-loader",
            // Translates CSS into CommonJS
            "css-loader",
            // Compiles Sass to CSS
            "sass-loader",
          ],
        },
        {
          test: /\.(woff|woff2|eot|ttf|otf)$/i,
          type: 'asset/resource',
        },
      ]
    },
    output: {
      filename: "main.js",
      path: outDir,
    },
    resolve: {
      modules: [
        path.resolve(__dirname, "node_modules"),
      ],
    },
  }
};