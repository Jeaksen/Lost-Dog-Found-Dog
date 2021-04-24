import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image } from 'react-native';
import AppIcon from '../Assets/AppIcon.png'

const {width, height} = Dimensions.get("screen")


export default class RegisterScreen extends React.Component {
  state={
    email: "",
    password: "",
    login: "",
    phone: "",
  }

  registerButton = ()=>{
    var login = this.state.login;
    var pass = this.state.password;
    var email = this.state.email;
    var phone = this.state.phone;
    console.log("login: "+ login + " pass: "+ pass+ " email"+email+" phone:"+ phone);
    try{
      fetch(this.props.Navi.URL + 'register', {
        method: 'POST', 
        headers: {
            'Content-Type': 'application/json',
            'Accept': '*/*'
        },
        body: JSON.stringify({userName: login, password: pass, email: email, phoneNumber: phone})
    })
    .then(response => {
      console.log("response:" + response)
      if (response.status == 404 || response.status == 401) {
          return null;
      }
      else if (response.status == 201) {
          return response.json();
        }
      else{
        return null;
      }
      })
      .then(responseData => {
        console.log("responseData: "+responseData)
        if (responseData != null) 
        {
          console.log(" SUCCESS !")
          this.props.Navi.swtichPage(1);
        } 
        else this.FailedRegister();
      })
      .catch(()=> this.FailedRegister())
      .finally(()=>console.log("LOADING ENDED"))
    }
    catch{
      FailedRegister();
    }
  }

  FailedRegister = () => {
    console.log("Register Failed");
  }

  render(){
    return(
        <View style={styles.content}>
          <Image source={AppIcon} style={styles.AppIcon}/>
          <Text style={styles.Title}>Register your account</Text>
          <TextInput style={styles.inputtext} placeholder="Login" onChangeText={(x) => this.setState({login: x})}/>
          <TextInput style={styles.inputtext} placeholder="Password" onChangeText={(x) => this.setState({password: x})}/>
          <TextInput style={styles.inputtext} placeholder="Email" onChangeText={(x) => this.setState({email: x})} />
          <TextInput style={styles.inputtext} placeholder="Phone number *" onChangeText={(x) => this.setState({phone: x})}/>
          <TouchableOpacity style={styles.loginButton} onPress={() => this.registerButton()}>
              <Text style={styles.logintext}>Sign in</Text>
          </TouchableOpacity>
            
        </View>
  )
  }
}


const styles = StyleSheet.create({
  AppIcon:{
    resizeMode: 'contain',
    aspectRatio: 1, 
    opacity: 0.8,
    width: 70,
    height: 70,
    alignSelf: 'center',
    marginBottom: 50,
  },
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
  marginBottom: 5,
  fontSize: 20,
  textAlign: 'center',
  fontWeight: 'bold',
}
});
