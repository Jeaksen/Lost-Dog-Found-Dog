import * as React from 'react';
import { View, StyleSheet,Dimensions,TouchableOpacity,FlatList, ShadowPropTypesIOS } from 'react-native';
import HeaderItem from './HeaderItem'

const {width, height} = Dimensions.get("screen")
const ExampleDATA = [{id: "1",title: "Login",},{id: "2",title: "Sign in",}];


export default class Header extends React.Component {
  state={
    DATA: [{id: "1",title: "Sign in",},{id: "2",title: "Sign up",}],
    SHOW: true,
  }

  show()
  {
    this.setState({SHOW: true})
  }
  hide()
  {
    this.setState({SHOW: false})
  }
  setList(newData)
  {
    this.setState({DATA: newData});
  }

  render(){
      var itemSize = this.props.headerHeight/2;
      var _butMargin = itemSize/10
      var L = this.state.DATA.length;
      var mainMargin = (width - ((L)*(itemSize+ 2*_butMargin)))/2;
    return(
        <View style={[styles.content,{}]}>
              {this.state.SHOW ?
              <FlatList
                    style={[{marginLeft: mainMargin}]}
                    contentContainerStyle={{flexGrow: 1, justifyContent: 'center',alignContent: 'center',}}
                    data={this.state.DATA}
                    numColumns={this.state.DATA.length}
                    key={this.state.DATA.length}
                    keyExtractor={(item) => item.id}
                    renderItem={({ item: button }) => <HeaderItem id={button.id} title={button.title} size={itemSize} butMargin={_butMargin} headerInput={this.props.headerInput}></HeaderItem>}
                />:null}
        </View>
  )
  }
}

const styles = StyleSheet.create({
  content: {
    height: height,
    width: width,
    flex: 1,
    alignSelf: 'center',
    justifyContent: 'center',
    marginVertical: 'auto',
  },

});
