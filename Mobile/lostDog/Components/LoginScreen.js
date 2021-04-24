import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import AppIcon from '../Assets/AppIcon.png'

const {width, height} = Dimensions.get("screen")


export default class LoginScreen extends React.Component {

  state={
    login: "popo",
    password: "SafePass66",
    loadingState: false,
  }


  loginButton = ()=>{
    const data = new FormData();
    data.append('username', this.state.login);
    data.append('password', this.state.password);

    this.props.Navi.RunOnBackend("login",data).then((responseData)=>{
      console.log(responseData)
      this.props.Navi.setToken(responseData.token,responseData.id);
      this.props.Navi.swtichPage(6);
    }).catch((x)=>
        console.log("Login Error" + (x))
      )
    return 0;

    var name = this.state.login;
    var pass = this.state.password;
    console.log("login: "+ name + " password: "+ pass);

    this.setState({loadingState: true})
    try{
      fetch(this.props.Navi.URL + 'login', {
        method: 'POST', 
        headers: {
            'Content-Type': 'application/json',
            'Accept': '*/*'
        },
        body: JSON.stringify({userName: name, password: pass})
    })
    .then(response => {
      if (response.status == 404 || response.status == 401) {
          return null;
      }
      else if (response.status == 200) {
          return response.json();
        }
      else{
        return null;
      }
      })
      .then(responseData => {
        if (responseData != null) 
        {
          console.log(" SUCCESS !")
          this.props.Navi.setToken(responseData.data.token,responseData.data.id);
          this.props.Navi.swtichPage(6);
        } 
        else this.FailedLogin();
      })
      .catch(()=> this.FailedLogin())
      .finally(()=>console.log("LOADING ENDED"))
    }
    catch{
      FailedLogin();
    }
  }

  FailedLogin = () => {
    console.log("Login Failed");
  }

  render(){
    return(
        <View style={styles.content}>
          <Image source={AppIcon} style={styles.AppIcon}/>
          <Text style={styles.Title}>Sign in</Text>
          <TextInput style={styles.inputtext} placeholder="Login" onChangeText={(x) => this.setState({login: x})}/>
          <TextInput style={styles.inputtext} placeholder="Password" onChangeText={(x) => this.setState({password: x})}/>
          {this.state.loadingState==false ?
            <TouchableOpacity style={styles.loginButton} onPress={() => this.loginButton()}>
              <Text style={styles.logintext}>Login</Text>
            </TouchableOpacity>
            :
            <Text>
                Loading ...
            </Text>
            }
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
},

});
