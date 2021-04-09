import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity } from 'react-native';

const {width, height} = Dimensions.get("screen")


export default class RegisterScreen extends React.Component {
  state={
    email: "",
    password: "",
    Name: "",
    Phone: "",
  }

  registerButton = ()=>{

    console.log("Register: "+ this.state.email + " password: "+ this.state.password + " password: "+ this.state.Name + " Phone: "+ this.state.Phone);
    this.props.Navi.swtichPage(0);
  }

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Lost Dog</Text>
          <TextInput style={styles.inputtext} placeholder="Email" onChangeText={(x) => this.setState({email: x})} />
          <TextInput style={styles.inputtext} placeholder="Password" onChangeText={(x) => this.setState({password: x})}/>
          <TextInput style={styles.inputtext} placeholder="Name and Surname *" onChangeText={(x) => this.setState({Name: x})}/>
          <TextInput style={styles.inputtext} placeholder="Phone number *" onChangeText={(x) => this.setState({Phone: x})}/>
          <TouchableOpacity style={styles.loginButton} onPress={() => this.registerButton()}>
              <Text style={styles.logintext}>Sign in</Text>
            </TouchableOpacity>
            
        </View>
  )
  }
}


const styles = StyleSheet.create({
  inputtext: {
    fontSize: 16,
    height: 30,
    width: width*0.5,
    borderColor: '#000000',
    borderWidth: 1,
    borderRadius: 5,
    paddingLeft: 5,
    marginVertical: 10,
  },
  content: {
    marginHorizontal: 30,
    height: '100%',
    alignSelf: 'center',
    justifyContent: 'center',
    marginVertical: 'auto',
  },
  loginButton:{
    marginTop: 20,
    backgroundColor: 'black',
    width: width*0.2,
    height: height*0.05,
    marginLeft: 'auto',
    marginRight: 'auto',
},
logintext:{
    marginTop: 'auto',
    marginBottom: 'auto',
    fontSize: 15,
    color: 'white',
    textAlign: 'center',
},
Title:{
  marginBottom: 50,
  fontSize: 20,
  textAlign: 'center',
  fontWeight: 'bold',
}
});
