const CopyPlugin = require('copy-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const Dotenv = require("dotenv-webpack")

const bundlePath = path.resolve(__dirname, 'build');

module.exports = (env, argv) => {
    const isDev = argv != undefined && argv.mode !== 'production';
    return {
        devServer: {
            static: bundlePath,
            open: false,
            hot: true,
            historyApiFallback: true,
        },
        entry: ['whatwg-fetch', './src/index.tsx'],
        output: {
            filename: isDev ? 'bundle.js' : 'bundle___[hash].js',
            path: bundlePath,
            publicPath: "/",
        },
        devtool: "inline-source-map",
        module: {
            rules: [
                {test: /\.(ts|tsx)$/, exclude: /node_modules/, loader: "ts-loader"},
                {test: /\.css$/, use: [MiniCssExtractPlugin.loader, "css-loader"]},
                {test: /\.(png|woff|woff2|eot|jpg|jpeg)$/, use: ['file-loader']},
                {
                    test: /\.svg$/,
                    exclude: /node_modules/,
                    loader: "svg-react-loader"
                },
                {
                    test: /\.less$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        {
                            loader: 'css-loader',
                            options: {
                                sourceMap: true,
                                modules: {
                                    localIdentName: "[name]__[local]___[hash:base64:5]",
                                },
                            }
                        },
                        'less-loader',
                    ],
                 },
            ]
        },
        plugins: [
            new Dotenv(),
            new MiniCssExtractPlugin({
                filename: isDev ? 'main.css' : 'main___[hash].css',
            }),
            new CopyPlugin(
                {patterns: [{from: 'resources',  globOptions: {
                    ignore: ["**/index.html"],
                  }, to: bundlePath}]}
            ),
            new HtmlWebpackPlugin({
                template: './resources/index.html',
            }),
        ],
        resolve: {
            extensions: ['.tsx', '.ts', '.js'],
            alias: {
                "src": path.resolve(__dirname, "src"),
            },
            fallback: { "querystring": require.resolve("querystring-es3") }
        }
    }
};
