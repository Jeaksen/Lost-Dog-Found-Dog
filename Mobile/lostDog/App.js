import * as React from 'react';
import { View, StyleSheet,Image, Text,ImageBackground,Dimensions  } from 'react-native';
import Navigator from './Components/Navigator';
import Background from './Components/Helpers/Background'

const {width, height} = Dimensions.get("screen")


export default class App extends React.Component {
  render() {
    return (
      <View style={styles.container}>
      <Background />
      <Navigator />
    </View>
    );
  }
}


const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: "column"
  },
  BackImage: {
    flex: 1,
    width: width,
    height: height,
    resizeMode: "cover",
    justifyContent: "center"
  },
  titleText: {
    fontSize: 23,
    fontWeight: "bold"
  },
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',

  },
});
