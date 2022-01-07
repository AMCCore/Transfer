let path = require("path");
let webpack = require("webpack");
var glob = require('glob');
const TerserJsPlugin = require("terser-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

function _path(p) {
    return path.join(__dirname, p);
}

let config = {
    entry: glob.sync('./Scripts/**/*.ts', {
        ignore: ['./Scripts/*.ts']
    }).reduce(function (obj, el) {
        let p = path.parse(el).dir.split('/').pop();
        (obj[p] = obj[p] || []).push(el);
        return obj
    }, {}),
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'),
        filename: '[name].js'
    },
    devtool: 'eval-source-map',
    resolve: {
        alias: {
            'jquery': _path('node_modules/jquery/dist/jquery')
        },
        extensions: ['.ts', '.tsx', '.js', '.jsx']
    },
    plugins: [
        new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }),
        new CleanWebpackPlugin(),
        new webpack.DefinePlugin({
            "require.specified": "require.resolve"
        })
    ],
    module: {
        rules: [
            {
                test: /\.css?$/,
                use: ['style-loader', 'css-loader']
            },
            { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
            { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' },
            {
                test: /\.(ts|js)$/, exclude: /node_modules/, use: {
                    loader: 'babel-loader', options: { presets: ['@babel/preset-env'] }
                }
            }
        ]
    }
};

module.exports = function (env, argv) {
    let isProd = argv.mode === 'production';
    if (isProd) {
        config.devtool = 'source-map';
        config.optimization = {
            minimize: true,
            minimizer: [new TerserJsPlugin()]
        };

        config.plugins.push(new MiniCssExtractPlugin({
            filename: '[name].css',

        }));
        config.module.rules[0].use = [MiniCssExtractPlugin.loader, "css-loader"];
    }

    return config;
};

