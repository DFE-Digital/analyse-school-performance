const path = require('path')
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyPlugin = require("copy-webpack-plugin");

const stylesConfig = {
    mode: process.env.NODE_ENV,
    entry: {
        app: path.resolve(__dirname, 'scripts/scss_imports.js')
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot'),    
    },
    module: {
        rules: [
            {
                test: /\.(css|scss)$/i,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            url: false
                        }
                    },
                    {
                        loader: 'postcss-loader'
                    },
                    {
                        loader: 'sass-loader',
                    }
                ]
            }
        ],
    },
    plugins: [
        new CopyPlugin({
            patterns: [
                { from: "node_modules/govuk-frontend/dist/govuk/assets", to: "assets" },
            ],
        }),
        new MiniCssExtractPlugin({
            filename: 'assets/css/[name].css'
        })
    ]
};

const javaScriptConfig = {
    mode: process.env.NODE_ENV,
    entry: {
        govuk: path.resolve(__dirname, 'scripts/js_imports.js'),
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/assets/js'),
        filename: '[name].js',
    }
};

const alpineConfig = {
    mode: process.env.NODE_ENV,
    entry: {
        alpine: path.resolve(__dirname, 'scripts/alpine.js'),
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/assets/js'),
        filename: '[name].js',
    }
};

module.exports = [stylesConfig, javaScriptConfig, alpineConfig];
